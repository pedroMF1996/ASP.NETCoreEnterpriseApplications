namespace NSE.WebApp.MVC.Models
{
    public class ErrorViewModel
    {
        public int ErroCode { get; set; }
        public string Titulo { get; set; }
        public string Mensagem {  get; set; }
    }

    public class ResponseResult
    {
        public string Title { get; set; }
        public int Status { get; set; }
        public ResponseErrorMensages Errors { get; set; }
        public ResponseResult()
        {
            Errors = new ResponseErrorMensages();
        }
    }

    public class ResponseErrorMensages
    {
        public List<string> Mensagens { get; set; }

        public ResponseErrorMensages()
        {
            Mensagens = new List<string>();
        }
    }
}