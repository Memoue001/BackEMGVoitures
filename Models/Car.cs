namespace EMGVoitures.Models
{
    public class Car
    {
        public int Id { get; set; }
        public string Make { get; set; }  // Marque de la voiture
        public string Model { get; set; }  // Modèle de la voiture
        public int Year { get; set; }  // Année de fabrication
        public string Description { get; set; }  // Description de la voiture
        public string PhotoUrl { get; set; }  // URL de la photo de la voiture
        public bool IsSold { get; set; }  // Si la voiture est vendue
    }
}
