namespace LogBookAPI.Models
{
    public class MentorItem : User
    {
        public int totalIntern { get; set; }
        public MentorItem(User user, int _totalIntern) : base(user){
            totalIntern = _totalIntern;
        }
    }
}