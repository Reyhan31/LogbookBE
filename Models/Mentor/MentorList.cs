namespace LogBookAPI.Models
{
    public class MentorList
    {

        public List<MentorItem> Data { get; set; }
        public int CurrentPage { get; set; }
        public int PageSize { get; set; }
        public int TotalPage { get; set; }
        public int TotalData { get; set; }
        public bool HasNext { get; set; }
        public bool HasPrevious { get; set; }
        public MentorList(){
        }

        public MentorList(MentorList obj)
        {
            Data = obj.Data;
            CurrentPage = obj.CurrentPage;
            PageSize = obj.PageSize;
            TotalPage = obj.TotalPage;
            TotalData = obj.TotalData;
            HasNext = obj.HasNext;
            HasPrevious = obj.HasPrevious;
        }
    }
}