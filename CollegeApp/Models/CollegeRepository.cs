namespace CollegeApp.Models
{
    public static class CollegeRepository
    {
        public static List<Student> Students { get; set; } = new List<Student>(){
                new Student { Id = 1, StudentName = "avc", Address = "yiih,guigo", Email = "fysgckjk@jhdkc.com" },
                new Student { Id = 2, StudentName = "iyj", Address = "tyikh,csia", Email = "xauo@yq.com" }
        };
    }
}