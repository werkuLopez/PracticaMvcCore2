namespace PracticaMvcCore2Iniciales.Helpers
{

    public enum Folder { Imagenes= 0}
    public class HelperPathProvider
    {
        private IWebHostEnvironment hostEnv;

        public HelperPathProvider(IWebHostEnvironment hostEnv)
        {
            this.hostEnv = hostEnv;
        }

        private string GetFolderPath(Folder folder)
        {
            string carpeta = "";

            if (folder == Folder.Imagenes)
            {
                carpeta = "images";
            }

            return carpeta;
        }
        public string MapPath(string fileName, Folder foldder)
        {
            string carpeta = this.GetFolderPath(foldder);
            string rootpath = this.hostEnv.WebRootPath;
            string path = Path.Combine(rootpath, carpeta, fileName);

            return path;
        }
    }
}
