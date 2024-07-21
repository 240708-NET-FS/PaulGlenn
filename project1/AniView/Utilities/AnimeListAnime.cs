
public class AnimeListAnime {
    public string url ; 
    public string title; 
    public string title_english; 
    public bool airing ;

    public int episodes; 

    public AnimeListAnime(string url, string title, string title_english, bool airing, Int32? episodes ) {
        this.url = url ; 
        this.title = title; 
        this.title_english = title_english;  
        this.airing = airing; 
        this.episodes = episodes ?? 0; 
    }

    private string isAiring() {
        if(this.airing) return "Yes";
        return "No"; 
    }

    public override string ToString() => $" {this.title_english} \n URL : {this.url} \n Episodes: {this.episodes} \n Airing: {this.isAiring() } \n --------------- \n ";
}