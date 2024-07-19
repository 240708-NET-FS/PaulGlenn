
class AnimeListAnime {
    public string url ; 
    public string title; 
    public string title_english; 
    public bool airing ;

    public int? episodes; 

    private string isAiring() {
        if(this.airing) return "Yes";
        return "No"; 
    }

    public override string ToString() => $" {this.title_english} \n URL : {this.url} \n Episodes: {this.episodes} \n Airing: {this.isAiring() } \n --------------- \n ";
}