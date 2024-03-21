namespace PracticaMvcCore2Iniciales.Helpers
{
    public class HelperUploadFiles
    {
        private HelperPathProvider helperProvider;

        public HelperUploadFiles(HelperPathProvider helperProvider)
        {
            this.helperProvider = helperProvider;
        }

        public async Task<string> UploadFileAsync(IFormFile file, Folder folder)
        {
            string fileName = file.FileName;
            string path = this.helperProvider.MapPath(fileName, folder);
            using (Stream st = new FileStream(path, FileMode.Create))
            {
                await file.CopyToAsync(st);
            }

            return path;
        }
    }
}
