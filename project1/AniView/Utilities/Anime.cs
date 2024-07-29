namespace AniView.Utilities;
public class Anime(string url, string title, string title_english, bool airing, Int32? episodes)
{
    public string url = url; 
    public string title = title; 
    public string title_english = title_english; 
    public bool airing = airing;

    public int episodes = episodes ?? 0;

    private string IsAiring() {
        if(this.airing) return "Yes";
        return "No"; 
    }

    public override string ToString() => $" {this.title_english ?? this.title} \n URL : {this.url} \n Episodes: {this.episodes} \n Airing: {this.IsAiring() } \n --------------- \n ";
}