﻿#nullable enable

using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Text.RegularExpressions;
using Uno.Extensions;
using Uno.UI;
using Uno.UI.Helpers.Xaml;
using Uno.UI.Xaml;
using Uno.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Documents;
using Windows.UI;
using Windows.Foundation;
using Windows.UI.Text;

#if XAMARIN_ANDROID
using _View = Android.Views.View;
#elif XAMARIN_IOS_UNIFIED
using _View = UIKit.UIView;
#else
using _View = Windows.UI.Xaml.UIElement;
#endif

namespace Windows.UI.Xaml.Markup.Reader
{
	internal class XamlObjectBuilder
	{
		private XamlFileDefinition _fileDefinition;
		private XamlTypeResolver TypeResolver { get; }
		private readonly List<(string elementName, ElementNameSubject bindingSubject)> _elementNames = new List<(string, ElementNameSubject)>();
		private readonly Stack<Type> _styleTargetTypeStack = new Stack<Type>();
		private Queue<Action> _postActions = new Queue<Action>();
		private static readonly Regex _attachedPropertMatch = new Regex(@"(\(.*?\))");

		private static Type[] _genericConvertibles = new []
		{
			typeof(Media.Brush),
			typeof(Media.SolidColorBrush),
			typeof(Color),
			typeof(Thickness),
			typeof(CornerRadius),
			typeof(Media.FontFamily),
			typeof(GridLength),
			typeof(Media.Animation.KeyTime),
			typeof(Duration),
			typeof(Media.Matrix),
			typeof(FontWeight),
		};

		public XamlObjectBuilder(XamlFileDefinition xamlFileDefinition)
		{
			_fileDefinition = xamlFileDefinition;
			TypeResolver = new XamlTypeResolver(_fileDefinition);
		}

		internal object? Build(object? component = null)
		{
			var topLevelControl = _fileDefinition.Objects.First();

			var instance = LoadObject(topLevelControl, component);

			ApplyPostActions(instance);

			return instance;
		}

		private object? LoadObject(XamlObjectDefinition? control, object? component = null)
		{
			if (control == null)
			{
				return null;
			}

			if (
				control.Type.Name == "NullExtension"
				&& control.Type.PreferredXamlNamespace == XamlConstants.XamlXmlNamespace
			)
			{
				return null;
			}

			var type = TypeResolver.FindType(control.Type);
			var classMember = control.Members.FirstOrDefault(m => m.Member.Name == "Class" && m.Member.PreferredXamlNamespace == XamlConstants.XamlXmlNamespace);

			if (type == null)
			{
				throw new InvalidOperationException($"Unable to find type {control.Type}");
			}

			var unknownContent = control.Members.Where(m => m.Member.Name == "_UnknownContent").FirstOrDefault();
			var unknownContentValue = unknownContent?.Value;
			var initializationMember = control.Members.Where(m => m.Member.Name == "_Initialization").FirstOrDefault();

			var isBrush = type == typeof(Media.Brush);

			if (type.Is<FrameworkTemplate>())
			{
				Func<_View?> builder = () =>
				{
					var contentOwner = unknownContent;

					return LoadObject(contentOwner?.Objects.FirstOrDefault()) as _View;
				};

				return Activator.CreateInstance(type, builder);
			}
			else if (type.Is<ResourceDictionary>())
			{
				var contentOwner = unknownContent;

				var rd = (ResourceDictionary)Activator.CreateInstance(type);
				foreach (var xamlObjectDefinition in contentOwner.Objects)
				{
					var key = xamlObjectDefinition.Members.FirstOrDefault(m => m.Member.Name == "Key")?.Value;

					var instance = LoadObject(xamlObjectDefinition);

					if (key != null)
					{
						rd.Add(key, instance);
					}
				}

				return rd;
			}
			else if (type.IsPrimitive && initializationMember?.Value is string primitiveValue)
			{
				return Convert.ChangeType(primitiveValue, type, CultureInfo.InvariantCulture);
			}
			else if (type == typeof(string) && unknownContentValue is string stringValue)
			{
				return stringValue;
			}
			else if (isBrush)
			{
				return XamlBindingHelper.ConvertValue(typeof(Media.Brush), unknownContentValue);
			}
			else
			{
				var classType = TypeResolver.FindType(classMember?.Value?.ToString());
				var instance = component ?? Activator.CreateInstance(type);

				IDisposable? TryProcessStyle()
				{
					if (instance is Style style)
					{
						if (control.Members.FirstOrDefault(m => m.Member.Name == "TargetType") is XamlMemberDefinition targetTypeDefinition)
						{
							if (BuildLiteralValue(targetTypeDefinition) is Type targetType)
							{
								_styleTargetTypeStack.Push(targetType);

								return Uno.Disposables.Disposable.Create(
									() =>
									{
										if (_styleTargetTypeStack.Pop() != targetType)
										{
											throw new InvalidOperationException("StyleTargetType is out of synchronization");
										}
									}
								);
							}
							else
							{
								throw new InvalidOperationException($"The type {targetTypeDefinition.Member.Type} is unknown");
							}
						}
					}

					return null;
				}

				using (TryProcessStyle())
				{
					foreach (var member in control.Members)
					{
						ProcessNamedMember(control, instance, member);
					}
				}

				return instance;
			}
		}

		private string RewriteAttachedPropertyPath(string? value)
		{
			value ??= "";

			if (value.Contains("("))
			{
				foreach (var ns in _fileDefinition.Namespaces)
				{
					if (ns != null)
					{
						var clrNamespace = ns.Namespace.TrimStart("using:");

						value = value.Replace("(" + ns.Prefix + ":", "(" + clrNamespace + ":");
					}
				}

				var match = _attachedPropertMatch.Match(value);

				if (match.Success)
				{
					do
					{
						if (!match.Value.Contains(":"))
						{
							// if there is no ":" this means that the type is using the default
							// namespace, so try to resolve the best way we can.

							var parts = match.Value.Trim(new[] { '(', ')' }).Split(new[] { '.' });

							if (parts.Length == 2)
							{
								var targetType = TypeResolver.FindType(parts[0]);

								if (targetType != null)
								{
									var newPath = targetType.Namespace + ":" + targetType.Name + "." + parts[1];

									value = value.Replace(match.Value, '(' + newPath + ')');
								}
							}
						}
					}
					while ((match = match.NextMatch()).Success);
				}
			}

			return value;
		}

		private void ProcessNamedMember(XamlObjectDefinition control, object instance, XamlMemberDefinition member)
		{
			// Exclude attached properties, must be set in the extended apply section.
			// If there is no type attached, this can be a binding.
			if (TypeResolver.IsType(control.Type, member.Member.DeclaringType)
				&& !TypeResolver.IsAttachedProperty(member)
				|| member.Member.Name == "_UnknownContent"
			// && FindEventType(member.Member) == null
			)
			{
				if (instance is TextBlock textBlock)
				{
					ProcessTextBlock(control, textBlock, member);
				}
				else if (instance is Documents.Span span && member.Member.Name == "_UnknownContent")
				{
					ProcessSpan(control, span, member);
				}
				else if (GetMemberProperty(control, member) is PropertyInfo propertyInfo)
				{
					if (member.Objects.None())
					{
						if (TypeResolver.IsInitializedCollection(propertyInfo)
							// The Resources property has a public setter, but should be treated as an empty collection
							|| IsResourcesProperty(propertyInfo)
						)
						{
							// Empty collection
						}
						else
						{
							if (propertyInfo.PropertyType == typeof(TargetPropertyPath))
							{
								ProcessTargetPropertyPath(instance, member, propertyInfo);
							}
							else
							{
								GetPropertySetter(propertyInfo).Invoke(instance, new[] { BuildLiteralValue(member, propertyInfo.PropertyType) });
							}
						}
					}
					else
					{
						if (IsMarkupExtension(member))
						{
							ProcessMemberMarkupExtension(instance, member, propertyInfo);
						}
						else
						{
							ProcessMemberElements(instance, member, propertyInfo);
						}
					}
				}
				else
				{
					// throw new InvalidOperationException($"The Property {member.Member.Name} does not exist on {member.Member.DeclaringType}");
				}
			}
			else if (TypeResolver.IsAttachedProperty(member))
			{
				var dependencyProperty = TypeResolver.FindDependencyProperty(member);

				if (dependencyProperty != null)
				{
					if (member.Objects.None())
					{
						(instance as DependencyObject)?.SetValue(dependencyProperty, BuildLiteralValue(member, dependencyProperty.Type));
					}
					else
					{
						if (IsMarkupExtension(member))
						{
							ProcessMemberMarkupExtension(instance, member, null);
						}
						else if (instance is DependencyObject dependencyObject)
						{
							ProcessMemberElements(dependencyObject, member, dependencyProperty);
						}
						else
						{
							throw new InvalidOperationException($"{instance} is not a DependencyObject");
						}
					}
				}
			}
			else if (member.Member.DeclaringType == null && member.Member.Name == "Name")
			{
				// This is a special case, where the declaring type is from the x: namespace,
				// but is considered of an unknown type. This can happen when providing the
				// name of a control using x:Name instead of Name.
				if (TypeResolver.GetPropertyByName(control.Type, "Name") is PropertyInfo nameInfo)
				{
					GetPropertySetter(nameInfo).Invoke(instance, new[] { member.Value });
				}
			}
		}

		private void ProcessSpan(XamlObjectDefinition control, Span span, XamlMemberDefinition member)
		{
			if (member.Objects.Count != 0)
			{
				foreach (var node in member.Objects)
				{
					if (LoadObject(node) is Inline inline)
					{
						span.Inlines.Add(inline);
					}
				}
			}

			if (member.Value != null)
			{
				span.Inlines.Add(
					new Run
					{
						Text = member.Value.ToString()
					}
				);
			}
		}

		private void ProcessTargetPropertyPath(object instance, XamlMemberDefinition member, PropertyInfo propertyInfo)
		{
			if (member.Value is string targetPath)
			{
				// This builds property setters for specified member setter.
				var separatorIndex = member.Value.ToString().IndexOf(".");
				var elementName = targetPath.Substring(0, separatorIndex);
				var propertyName = targetPath.Substring(separatorIndex + 1);

				if (instance is Setter setter)
				{
					setter.Target = new TargetPropertyPath(elementName, new PropertyPath(RewriteAttachedPropertyPath(propertyName)));
				}
			}
			else
			{
				throw new NotSupportedException($"The property {propertyInfo} must be provided a value");
			}
		}

		private void ProcessTextBlock(XamlObjectDefinition control, TextBlock instance, XamlMemberDefinition member)
		{
			if (member.Objects.Any())
			{
				if (IsMarkupExtension(member))
				{
					ProcessMemberMarkupExtension(instance, member, null);
				}
				else
				{
					foreach (var node in member.Objects)
					{
						if (LoadObject(node) is Inline inline)
						{
							instance.Inlines.Add(inline);
						}
					}
				}
			}
			else if (GetMemberProperty(control, member) is PropertyInfo propertyInfo)
			{
				GetPropertySetter(propertyInfo).Invoke(instance, new[] { BuildLiteralValue(member, propertyInfo.PropertyType) });
			}
		}

		private void ProcessMemberElements(DependencyObject instance, XamlMemberDefinition member, DependencyProperty property)
		{
			if (TypeResolver.IsCollectionOrListType(property.Type))
			{
				object BuildInstance()
				{
					if (property.Type.GetGenericTypeDefinition() == typeof(IList<>))
					{
						return Activator.CreateInstance(typeof(List<>).MakeGenericType(property.Type.GenericTypeArguments[0]));
					}
					else
					{
						return Activator.CreateInstance(property.Type);
					}
				}

				var collection = BuildInstance();

				AddCollectionItems(collection, member.Objects);

				instance.SetValue(property, collection);
			}
			else
			{
				instance.SetValue(property, LoadObject(member.Objects.First()));
			}
		}

		private void ProcessMemberElements(object instance, XamlMemberDefinition member, PropertyInfo propertyInfo)
		{
			if (TypeResolver.IsCollectionOrListType(propertyInfo.PropertyType))
			{
				if (propertyInfo.PropertyType == typeof(ResourceDictionary))
				{
					var methods = propertyInfo.PropertyType.GetMethods();
					var addMethod = propertyInfo.PropertyType.GetMethod("Add", new[] { typeof(object), typeof(object) });

					foreach (var child in member.Objects)
					{
						var item = LoadObject(child);

						var resourceKey = GetResourceKey(child);
						var resourceTargetType = GetResourceTargetType(child);

						if (
							item?.GetType() == typeof(Style)
							&& resourceTargetType == null
						)
						{
							throw new InvalidOperationException($"No target type was specified (Line {member.LineNumber}:{member.LinePosition}");
						}

						var propertyInstance = propertyInfo.GetMethod.Invoke(instance, null);

						addMethod.Invoke(propertyInstance, new[] { resourceKey ?? resourceTargetType, item });
					}
				}
				else if (TypeResolver.IsNewableProperty(propertyInfo, out var collectionType))
				{
					var collection = Activator.CreateInstance(collectionType);

					AddCollectionItems(collection, member.Objects);

					GetPropertySetter(propertyInfo).Invoke(instance, new[] { collection });
				}
				else if (TypeResolver.IsInitializedCollection(propertyInfo))
				{
					var propertyInstance = propertyInfo.GetMethod.Invoke(instance, null);

					AddCollectionItems(propertyInstance, member.Objects);
				}
				else
				{
					throw new InvalidOperationException($"Unsupported collection type {propertyInfo.PropertyType} on {propertyInfo}");
				}
			}
			else
			{
				GetPropertySetter(propertyInfo).Invoke(instance, new[] { LoadObject(member.Objects.First()) });
			}
		}

		private static MethodInfo GetPropertySetter(PropertyInfo propertyInfo)
			=> propertyInfo?.SetMethod ?? throw new InvalidOperationException($"Unable to find setter for property [{propertyInfo}]");

		private void ProcessMemberMarkupExtension(object instance, XamlMemberDefinition member, PropertyInfo propertyInfo)
		{
			if (IsBindingMarkupNode(member))
			{
				ProcessBindingMarkupNode(instance, member);
			}
			else if (IsStaticResourceMarkupNode(member) || IsThemeResourceMarkupNode(member))
			{
				ProcessStaticResourceMarkupNode(instance, member, propertyInfo);
			}
		}

		private void ProcessStaticResourceMarkupNode(object instance, XamlMemberDefinition member, PropertyInfo propertyInfo)
		{
			var resourceNode = member.Objects.FirstOrDefault();

			if (resourceNode != null)
			{
				var keyName = resourceNode.Members.FirstOrDefault()?.Value?.ToString();
				var dependencyProperty = TypeResolver.FindDependencyProperty(member);

				if (keyName != null
					&& dependencyProperty != null
					&& instance is DependencyObject dependencyObject)
				{
					ResourceResolver.ApplyResource(
						dependencyObject,
						dependencyProperty,
						keyName,
						isThemeResourceExtension: IsThemeResourceMarkupNode(member),
						isHotReloadSupported: true);

					if (instance is FrameworkElement fe)
					{
						fe.Loading += delegate
						{
							fe.UpdateResourceBindings();
						};
					}
				}
				else if (propertyInfo != null)
				{
					GetPropertySetter(propertyInfo).Invoke(
								instance,
								new[] { ResourceResolver.ResolveResourceStatic(keyName, propertyInfo.PropertyType) }
							);

					if (instance is Setter setter && propertyInfo.Name == "Value")
					{
						// Register StaticResource/ThemeResource assignations to Value for resource updates
						setter.ApplyThemeResourceUpdateValues(
							keyName,
							null,
							IsThemeResourceMarkupNode(member),
							true
						);
					}
				}
			}
		}

		private static object? ResolveStaticResource(object? instance, string? keyName)
		{
			var staticResource = (instance as FrameworkElement)
					.Flatten(i => (i?.Parent as FrameworkElement))
					.Select(fe =>
					{
						if (fe != null
							&& fe.Resources.TryGetValue(keyName, out var resource, shouldCheckSystem: false))
						{
							return resource;
						}
						return null;
					})
					.Concat(ResourceResolver.ResolveTopLevelResource(keyName))
					.Trim()
					.FirstOrDefault();

			return staticResource;
		}

		private bool IsStaticResourceMarkupNode(XamlMemberDefinition member)
			=> member.Objects.Any(o => o.Type.Name == "StaticResource");

		private bool IsThemeResourceMarkupNode(XamlMemberDefinition member)
			=> member.Objects.Any(o => o.Type.Name == "ThemeResource");

		private bool IsResourcesProperty(PropertyInfo propertyInfo)
			=> propertyInfo.Name == "Resources" && propertyInfo.PropertyType == typeof(ResourceDictionary);

		private void ProcessBindingMarkupNode(object instance, XamlMemberDefinition member)
		{
			var binding = BuildBindingExpression(instance, member);

			if (instance is IDependencyObjectStoreProvider provider)
			{
				var dependencyProperty = TypeResolver.FindDependencyProperty(member);

				if (dependencyProperty != null)
				{
					provider.Store.SetBinding(dependencyProperty, binding);
				}
				else if (TypeResolver.GetPropertyByName(member.Owner.Type, member.Member.Name) is PropertyInfo propertyInfo)
				{
					if (member.Objects.Empty())
					{
						GetPropertySetter(propertyInfo).Invoke(instance, new[] { BuildLiteralValue(member, propertyInfo.PropertyType) });
					}
					else
					{
						GetPropertySetter(propertyInfo).Invoke(instance, new[] { BuildBindingExpression(null, member) });
					}
				}
				else
				{
					throw new NotSupportedException($"Unknown dependency property {member.Member}");
				}
			}
			else
			{
				throw new NotSupportedException($"Binding is not supported on {member.Member}");
			}
		}

		private Binding BuildBindingExpression(object? instance, XamlMemberDefinition member)
		{
			var bindingNode = member.Objects.FirstOrDefault(o => o.Type.Name == "Binding");
			var templateBindingNode = member.Objects.FirstOrDefault(o => o.Type.Name == "TemplateBinding");

			var binding = new Data.Binding();

			if (templateBindingNode != null)
			{
				binding.RelativeSource = RelativeSource.TemplatedParent;
			}

			if(bindingNode == null && templateBindingNode == null)
			{
				throw new InvalidOperationException("Unable to find Binding or TemplateBinding node");
			}

			foreach (var bindingProperty in (bindingNode ?? templateBindingNode)!.Members)
			{
				switch (bindingProperty.Member.Name)
				{
					case "_PositionalParameters":
					case nameof(Binding.Path):
						binding.Path = RewriteAttachedPropertyPath(bindingProperty.Value?.ToString());
						break;

					case nameof(Binding.ElementName):
						var subject = new ElementNameSubject();
						binding.ElementName = subject;

						if (bindingProperty.Value != null)
						{
							AddElementName(bindingProperty.Value.ToString(), subject);
						}
						break;

					case nameof(Binding.TargetNullValue):
						binding.TargetNullValue = bindingProperty.Value?.ToString();
						break;

					case nameof(Binding.FallbackValue):
						binding.FallbackValue = bindingProperty.Value?.ToString();
						break;

					case nameof(Binding.UpdateSourceTrigger):
						if (Enum.TryParse<UpdateSourceTrigger>(bindingProperty.Value?.ToString(), out var trigger))
						{
							binding.UpdateSourceTrigger = trigger;
						}
						else
						{
							throw new NotSupportedException($"Invalid binding mode {bindingProperty.Value}");
						}
						break;

					case nameof(Binding.RelativeSource):
						if (bindingProperty.Objects.First() is XamlObjectDefinition relativeSource && relativeSource.Type.Name == "RelativeSource")
						{
							var relativeSourceValue = relativeSource.Members.FirstOrDefault()?.Value?.ToString()?.ToLowerInvariant();
							switch (relativeSourceValue)
							{
								case "templatedparent":
									binding.RelativeSource = RelativeSource.TemplatedParent;
									break;

								default:
									throw new NotSupportedException($"RelativeSource {relativeSourceValue} is not supported");
							}
						}
						break;

					case nameof(Binding.Mode):
						if (Enum.TryParse<Data.BindingMode>(bindingProperty.Value?.ToString(), out var mode))
						{
							binding.Mode = mode;
						}
						else
						{
							throw new NotSupportedException($"Invalid binding mode {bindingProperty.Value}");
						}
						break;

					case nameof(Binding.ConverterParameter):
						binding.ConverterParameter = bindingProperty.Value?.ToString();
						break;

					case nameof(Binding.Converter):
						if (
							bindingProperty.Objects.First() is XamlObjectDefinition converterResource
						)
						{
							if (converterResource.Type.Name == "StaticResource")
							{
								var staticResourceName = converterResource.Members.FirstOrDefault()?.Value?.ToString();

								void ResolveResource()
								{
									var staticResource = ResolveStaticResource(instance, staticResourceName);

									if (staticResource != null)
									{
										binding.Converter = staticResource as IValueConverter;
									}
								}

								_postActions.Enqueue(ResolveResource);
							}
							else
							{
								throw new NotSupportedException($"Markup extension {converterResource.Type.Name} is not supported for Bindiner.Converter");
							}
						}
						break;

					default:
						throw new NotSupportedException($"Binding option {bindingProperty.Member} is not supported");
				}
			}

			return binding;
		}

		private void AddElementName(string elementName, ElementNameSubject subject)
		{
			_elementNames.Add((elementName, subject));
		}

		private bool IsBindingMarkupNode(XamlMemberDefinition member)
			=> member.Objects.Any(o => o.Type.Name == "Binding" || o.Type.Name == "TemplateBinding");

		private static bool IsMarkupExtension(XamlMemberDefinition member)
			=> member
				.Objects
				.Where(m =>
					m.Type.Name == "Binding"
					|| m.Type.Name == "Bind"
					|| m.Type.Name == "StaticResource"
					|| m.Type.Name == "ThemeResource"
					|| m.Type.Name == "TemplateBinding"
				)
				.Any();

		private void AddCollectionItems(object collectionInstance, IEnumerable<XamlObjectDefinition> nonBindingObjects)
		{
			var addMethod = collectionInstance.GetType().GetMethod("Add");

			foreach (var child in nonBindingObjects)
			{
				var item = LoadObject(child);

				addMethod.Invoke(collectionInstance, new[] { item });
			}
		}

		private object? GetResourceKey(XamlObjectDefinition child) =>
			child.Members.FirstOrDefault(m =>
				string.Equals(m.Member.Name, "Name", StringComparison.OrdinalIgnoreCase)
				|| string.Equals(m.Member.Name, "Key", StringComparison.OrdinalIgnoreCase)
			)
			?.Value
			?.ToString();

		private Type GetResourceTargetType(XamlObjectDefinition child) =>
				TypeResolver.FindType(child.Members.FirstOrDefault(m =>
					string.Equals(m.Member.Name, "TargetType", StringComparison.OrdinalIgnoreCase)
				)
				?.Value
				?.ToString() ?? ""
			);

		private PropertyInfo GetMemberProperty(XamlObjectDefinition control, XamlMemberDefinition member)
		{
			if (member.Member.Name == "_UnknownContent")
			{
				var property = TypeResolver.FindContentProperty(TypeResolver.FindType(control.Type));

				if (property == null)
				{
					throw new InvalidOperationException($"Implicit content is not supported on {control.Type}");
				}

				return property;
			}
			else
			{
				return TypeResolver.GetPropertyByName(control.Type, member.Member.Name);
			}
		}

		private object? BuildLiteralValue(XamlMemberDefinition member, Type? propertyType = null)
		{
			if (member.Objects.None())
			{
				var memberValue = member.Value?.ToString();

				propertyType = propertyType ?? TypeResolver.FindPropertyType(member.Member);

				if (propertyType != null)
				{
					if (propertyType == typeof(Type))
					{
						return TypeResolver.FindType(memberValue);
					}
					else if (propertyType == typeof(DependencyProperty) && member.Owner.Type.Name == "Setter")
					{
						var propertyOwner = _styleTargetTypeStack.Peek();

						if (TypeResolver.FindDependencyProperty(propertyOwner, memberValue) is DependencyProperty property)
						{
							return property;
						}
						else
						{
							throw new Exception($"The property {propertyOwner}.{memberValue} does not exist");
						}
					}
					else
					{
						return BuildLiteralValue(propertyType, memberValue);
					}
				}
				else
				{
					throw new Exception($"The property {member.Owner?.Type?.Name}.{member.Member?.Name} is unknown");
				}
			}
			else
			{
				var expression = member.Objects.First();

				throw new NotSupportedException("MarkupExtension {0} is not supported.".InvariantCultureFormat(expression.Type.Name));
			}
		}

		private object? BuildLiteralValue(Type propertyType, string? memberValue)
		{
			return Uno.UI.DataBinding.BindingPropertyHelper.Convert(() => propertyType, memberValue);
		}

		private void ApplyPostActions(object? instance)
		{
			if (instance is FrameworkElement fe)
			{
				ResolveElementNames(fe);
			}

			while (_postActions.Count != 0)
			{
				_postActions.Dequeue()();
			}
		}

		private void ResolveElementNames(FrameworkElement root)
		{
			foreach (var (elementName, bindingSubject) in _elementNames)
			{
				if (root.FindName(elementName) is DependencyObject element)
				{
					bindingSubject.ElementInstance = element;
				}
			}
		}
	}
}
