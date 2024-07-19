namespace CatApp.Entities; 

public class Cat {
    public int CatID {get;set; }
    public string Tags {get; set; }

    public int UserID {get;set; }

    public User User {get; set; }
    public DateTime DateRequested {get; set ; }

    public bool Favorite {get;set; }

}