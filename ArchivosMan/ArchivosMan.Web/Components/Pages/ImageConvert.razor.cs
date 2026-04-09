using Microsoft.AspNetCore.Components.Forms;
using Microsoft.JSInterop;

namespace ArchivosMan.Web.Components.Pages
{
    public partial class ImageConvert
    {
        private string? PreviewImage;
        private string? DownloadUrl;
        private string TargetFormat = "png";
        private IBrowserFile? UploadedFile;

        private async Task OnInputFileChange(InputFileChangeEventArgs e)
        {
            UploadedFile = e.File;
            var buffer = new byte[UploadedFile.Size];
            await UploadedFile.OpenReadStream().ReadExactlyAsync(buffer);
            PreviewImage = $"data:{UploadedFile.ContentType};base64,{Convert.ToBase64String(buffer)}";
            DownloadUrl = null;
        }

        private async Task ConvertImage()
        {
            if (PreviewImage is null || UploadedFile is null)
                return;

            DownloadUrl = await JS.InvokeAsync<string>("convertImage", PreviewImage, TargetFormat);
        }
    }
}
