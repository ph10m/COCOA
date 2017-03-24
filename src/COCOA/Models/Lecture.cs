namespace COCOA.Models
{
    /// <summary>
    /// Needed information about each lecture
    /// </summary>
    public class Lecture
    {
        private string day, time, course, startTime;
        public Lecture(string course, string day, string time)
        {
            this.day = day;
            this.time = time;
            this.course = course;
            this.startTime = time.Split('-')[0];
        }
        public string getDay() { return this.day; }
        /// <summary>
        /// Convert time to the format: HH.MM.SS
        /// </summary>
        public string getFormattedTime() { return this.startTime.Replace(':', '.') + ".00"; }

        public override string ToString()
        {
            return "Class: " + this.course + "\nDay: " + this.day + "\nTime: " + this.time + "\n";
        }
    }

}
