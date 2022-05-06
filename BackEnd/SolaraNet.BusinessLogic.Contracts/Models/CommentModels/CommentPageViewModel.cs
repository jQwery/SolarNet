namespace SolaraNet.BusinessLogic.Contracts.Models
{
    public class CommentPageViewModel
    {
        public int AdvertismentId { get; set; }
        public int CommentsPerPage { get; set; } // предполагается, что это количество выбирает пользователь на фронте
        public int Offset { get; set; }

        public CommentPageViewModel(int advertismentId, int commentsPerPage, int offset)
        {
            AdvertismentId = advertismentId;
            CommentsPerPage = commentsPerPage;
            Offset = offset;
        }

    }
}