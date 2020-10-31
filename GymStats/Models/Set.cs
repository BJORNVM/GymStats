namespace GymStats.Models
{
    class Set
    {
        public int ID { get; set; }
        public int ExerciseID { get; set; }
        public double Weight { get; set; }
        public int Intensity { get; set; }
        public int Repetitions { get; set; }

        public double Volume => Weight * Repetitions;
    }
}
