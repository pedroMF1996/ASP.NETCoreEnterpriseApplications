namespace NSE.WebApp.MVC.Models
{
    public class ErrorViewModel
    {
        public string? RequestId { get; set; }

        public bool ShowRequestId => !string.IsNullOrEmpty(RequestId);
    }

    public class ResponseResult
    {
        public string Title { get; set; }
        public int Status { get; set; }
        public ResponseErrorMensages Errors { get; set; }
    }

    public class ResponseErrorMensages
    {
        public List<string> Mensagens { get; set; }
    }
}