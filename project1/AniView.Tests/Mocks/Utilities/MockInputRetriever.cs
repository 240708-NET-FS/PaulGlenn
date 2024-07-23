using AniView.Utilities; 

public class MockInputRetriever : InputRetriever {
    // private int _nameIndex =  0;
    // private int _choiceIndex= 0; 

    // private string[] mockNameList = {"My Hero Academia","FakeAnime"};  

    // private int[] choiceIndex = {1,500};
    public string mockName {get; set; }
    public int mockChoice {get; set; }
    public override string GetName()
    {
        return mockName; 
    }

    public override int GetChoice()
    {
        return mockChoice; 
    }
}