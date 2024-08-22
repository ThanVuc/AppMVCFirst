namespace AppMVC.Models
{
    public class SummernoteSetting
    {
        public SummernoteSetting(string textAreaID, bool loadLibraries = true, string placeHolder = "")
        {
            TextAreaID = textAreaID;
            LoadLibraries = loadLibraries;
            PlaceHolder = placeHolder;
        }

        public string TextAreaID { get; set; }
        public string PlaceHolder { get; set; } = "";
        public bool LoadLibraries { get; set; }
        public int Height { get; set; } = 120;
        public string ToolBar { get; set; } = @"[
                ['style', ['style']],
                ['font', ['bold', 'underline', 'clear']],
                ['color', ['color']],
                ['para', ['ul', 'ol', 'paragraph']],
                ['table', ['table']],
                ['insert', ['link', 'picture', 'video']],
                ['view', ['fullscreen', 'codeview', 'help']]
            ]";


    }
}
