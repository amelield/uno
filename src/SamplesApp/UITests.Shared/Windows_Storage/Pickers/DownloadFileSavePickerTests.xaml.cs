using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Threading.Tasks;
using Uno;
using Uno.UI.Samples.Controls;
using Windows.Storage;
using Windows.Storage.Pickers;
using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;

namespace UITests.Windows_Storage.Pickers
{
	[Sample("Windows.Storage", IsManualTest = true, Description = "This sample is specifically suited for WASM, should trigger a download of a given file")]
    public sealed partial class DownloadFileSavePickerTests : Page
    {
        public DownloadFileSavePickerTests()
        {
            this.InitializeComponent();
			this.Loaded += DownloadFileSavePickerTests_Loaded;
			this.Unloaded += DownloadFileSavePickerTests_Unloaded;
        }

		private void DownloadFileSavePickerTests_Unloaded(object sender, RoutedEventArgs e)
		{
#if __WASM__
			WinRTFeatureConfiguration.Storage.Pickers.WasmConfiguration = WasmPickerConfiguration.FileSystemAccessApiWithFallback;
#endif
		}

		private void DownloadFileSavePickerTests_Loaded(object sender, RoutedEventArgs e)
		{
#if __WASM__
			WinRTFeatureConfiguration.Storage.Pickers.WasmConfiguration = WasmPickerConfiguration.DownloadUpload;
#endif
		}

		private enum FileType
		{
			Txt,
			Pdf,
			Img
		}

		private async void SavePdfButton_Click(object sender, RoutedEventArgs e) => await SaveFile(FileType.Pdf);

		private async void SaveImgButton_Click(object sender, RoutedEventArgs e) => await SaveFile(FileType.Img);

		private async void SaveTxtButton_Click(object sender, RoutedEventArgs e) => await SaveFile(FileType.Txt);

		private async Task SaveFile(FileType fileType)
		{

			var actionsText = new StringBuilder();

			if (fileType == FileType.Img) { await WriteImageToFile(); }
			else if (fileType == FileType.Pdf) { await WritePDFToFile(); }
			else { await WriteToFile(); }

			actionsText.AppendLine("Written to file.");

			OutputTextBlock.Text = actionsText.ToString();
			actionsText.Clear();
		}

		private void UpdateFileStatus(StorageFile file)
		{
			FileName.Text = file.Name;
			FilePath.Text = file.Path;
		}

		private async Task WriteToFile()
		{
			var file = await GetFile("plain/text", ".txt");
			UpdateFileStatus(file);
			FileUpdateStatus.Text = "Deferring";
			CachedFileManager.DeferUpdates(file);

			var stream = await file.OpenStreamForWriteAsync();

			using (var writer = new StreamWriter(stream))
			{
				writer.Write("Hi, this is the content of the file.");
			}

			var status = await CachedFileManager.CompleteUpdatesAsync(file);
			FileUpdateStatus.Text = status.ToString();
		}

		private async Task WriteImageToFile()
		{
			var file = await GetFile("image/png", ".png");
			UpdateFileStatus(file);
			FileUpdateStatus.Text = "Deferring";
			CachedFileManager.DeferUpdates(file);

			var base64String = "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";
			var data = Convert.FromBase64String(base64String);

			var stream = await file.OpenStreamForWriteAsync();
			using (var writer = new BinaryWriter(stream))
			{
				for (var i = 0; i < data.Length; i++)
				{
					writer.Write(data[i]);
				}
			}
			var status = await CachedFileManager.CompleteUpdatesAsync(file);
			FileUpdateStatus.Text = status.ToString();
		}

		private async Task WritePDFToFile()
		{
			var file = await GetFile("application/pdf", ".pdf");
			UpdateFileStatus(file);
			FileUpdateStatus.Text = "Deferring";
			CachedFileManager.DeferUpdates(file);

			var base64String = "JVBERi0yLjAKJbrR8akKMSAwIG9iajw8L1R5cGUvQ2F0YWxvZy9QYWdlcyAyIDAgUi9NZXRhZGF0YSAxMiAwIFI+PgplbmRvYmoKMiAwIG9iajw8L1R5cGUvUGFnZXMvS2lkc1szIDAgUl0vQ291bnQgMT4+CmVuZG9iagoxMiAwIG9iajw8L1R5cGUvTWV0YWRhdGEvU3VidHlwZS9YTUwvTGVuZ3RoIDMxNzU+PnN0cmVhbQo8P3hwYWNrZXQgYmVnaW49Iu+7vyIgaWQ9Ilc1TTBNcENlaGlIenJlU3pOVGN6a2M5ZCI/Pgo8eDp4bXBtZXRhIHhtbG5zOng9ImFkb2JlOm5zOm1ldGEvIiB4OnhtcHRrPSJYTVAgQ29yZSA0LjQuMCI+CiAgIDxyZGY6UkRGIHhtbG5zOnJkZj0iaHR0cDovL3d3dy53My5vcmcvMTk5OS8wMi8yMi1yZGYtc3ludGF4LW5zIyI+CiAgICAgIDxyZGY6RGVzY3JpcHRpb24gcmRmOmFib3V0PSIiCiAgICAgICAgICAgIHhtbG5zOmRjPSJodHRwOi8vcHVybC5vcmcvZGMvZWxlbWVudHMvMS4xLyI+CiAgICAgICAgIDxkYzpmb3JtYXQ+YXBwbGljYXRpb24vcGRmPC9kYzpmb3JtYXQ+CiAgICAgICAgIDxkYzp0aXRsZT4KICAgICAgICAgICAgPHJkZjpBbHQ+CiAgICAgICAgICAgICAgIDxyZGY6bGkgeG1sOmxhbmc9IngtZGVmYXVsdCI+Mzk3YmUyMmUtNmFkZC00NzVhLWE1NTEtNGYwNGY0NTIxYjY5LnBkZjwvcmRmOmxpPgogICAgICAgICAgICA8L3JkZjpBbHQ+CiAgICAgICAgIDwvZGM6dGl0bGU+CiAgICAgICAgIDxkYzpjcmVhdG9yPgogICAgICAgICAgICA8cmRmOlNlcT4KICAgICAgICAgICAgICAgPHJkZjpsaT5Tb2RhIFBERiBPbmxpbmU8L3JkZjpsaT4KICAgICAgICAgICAgPC9yZGY6U2VxPgogICAgICAgICA8L2RjOmNyZWF0b3I+CiAgICAgIDwvcmRmOkRlc2NyaXB0aW9uPgogICAgICA8cmRmOkRlc2NyaXB0aW9uIHJkZjphYm91dD0iIgogICAgICAgICAgICB4bWxuczp4bXA9Imh0dHA6Ly9ucy5hZG9iZS5jb20veGFwLzEuMC8iPgogICAgICAgICA8eG1wOk1vZGlmeURhdGU+MjAyMC0wNy0yM1QxNzo0NDo0NS0wNDowMDwveG1wOk1vZGlmeURhdGU+CiAgICAgICAgIDx4bXA6Q3JlYXRvclRvb2w+U29kYSBQREYgT25saW5lPC94bXA6Q3JlYXRvclRvb2w+CiAgICAgIDwvcmRmOkRlc2NyaXB0aW9uPgogICAgICA8cmRmOkRlc2NyaXB0aW9uIHJkZjphYm91dD0iIgogICAgICAgICAgICB4bWxuczpwZGY9Imh0dHA6Ly9ucy5hZG9iZS5jb20vcGRmLzEuMy8iPgogICAgICAgICA8cGRmOlByb2R1Y2VyPlNvZGEgUERGIE9ubGluZTwvcGRmOlByb2R1Y2VyPgogICAgICA8L3JkZjpEZXNjcmlwdGlvbj4KICAgPC9yZGY6UkRGPgo8L3g6eG1wbWV0YT4KICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgIAogICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgCiAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAKICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgIAogICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgCiAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAKICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgIAogICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgCiAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAKICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgIAogICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgCiAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAKICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgIAogICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgCiAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAKICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgIAogICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgCiAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAKICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgIAogICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgCiAgICAgICAgICAgICAgICAgICAgICAgICAgIAo8P3hwYWNrZXQgZW5kPSJ3Ij8+CmVuZHN0cmVhbQplbmRvYmoKMyAwIG9iajw8L1R5cGUvUGFnZS9QYXJlbnQgMiAwIFIvTWVkaWFCb3hbMCAwIDYxMiA3OTJdL1Jlc291cmNlczw8L0ZvbnQ8PC9GMCA0IDAgUj4+Pj4vQ3JvcEJveFswIDAgNjEyIDc5Ml0vQ29udGVudHMgNSAwIFI+PgplbmRvYmoKNCAwIG9iajw8L1R5cGUvRm9udC9CYXNlRm9udC9XRFZESlErVGltZXNOZXdSb21hblBTTVQvVG9Vbmljb2RlIDEzIDAgUi9TdWJ0eXBlL1R5cGUwL0Rlc2NlbmRh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";
			var data = Convert.FromBase64String(base64String);

			var stream = await file.OpenStreamForWriteAsync();
			using (var writer = new BinaryWriter(stream))
			{
				for (var i = 0; i < data.Length; i++)
				{
					writer.Write(data[i]);
				}
			}
			var status = await CachedFileManager.CompleteUpdatesAsync(file);
			FileUpdateStatus.Text = status.ToString();
		}

		private static async Task<StorageFile> GetFile(string mimeType, string extension)
		{
			var savePicker = new FileSavePicker { SuggestedStartLocation = PickerLocationId.DocumentsLibrary };
			savePicker.FileTypeChoices.Add(mimeType, new List<string>() { extension });
			savePicker.SuggestedFileName = "New Document";
			var file = await savePicker.PickSaveFileAsync();
			return file;
		}
	}
}
