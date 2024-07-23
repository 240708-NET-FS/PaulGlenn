namespace AniView.Entities; 

public class User {
    public int UserID {get; set; }

    public string UserName {get; set; }

    public string Passphrase {get; set; }

    public virtual ICollection<Show> Shows {get; set; }
}