using UnityEngine;
using System.Collections.Generic;

class Quests {

    public static List<string> GetQuest(int questNum) {
        switch (questNum) {
            case 0:
                return new List<string> {"Toilet", "Sink", "Bathtub"};
            case 1:
                return new List<string> {"Socks", "Jeans", "Shirt"};
            case 2:
                return new List<string> {"Fridge", "Grill", "Whirlpool"};
            case 3:
                return new List<string> {"Parrot", "Cage"};
            case 4:
                return new List<string> {"Sink", "Flower"};
            default: 
                return null;
        }
    }

    public static string GetQuestName(int questNum) {
        switch (questNum) {
            case 0:
                return "Morning Hygiene";
            case 1:
                return "Dress Up!";
            case 2:
                return "Cook BBQ Breakfast";
            case 3:
                return "Catch the Pet";
            case 4:
                return "Water the Plant";
            default: 
                return null;
        }
    }

    public static void QuestProgressCallback(int currentQuest, int progressDone) {
        //                                   ^^^params^^^ are before update

        if (currentQuest == 0 || currentQuest == 2) {
            // Simply play some sound, no matter what thing it is
            Thing thing = GameObject.Find(GetQuest(currentQuest)[progressDone]).GetComponent<Thing>();
            thing.PlaySound();

        } else if (currentQuest == 1) {
            // Remove cloth thing
            GameObject socksObj = GameObject.Find(GetQuest(currentQuest)[progressDone]);
            GameObject.Destroy(socksObj);

            // And set human clothes level
            Human.Instance.SetClothesLevel(++progressDone);

        } else if (currentQuest == 3) {

            GameObject parrotObj = GameObject.Find("Parrot");
            if (progressDone == 0) {
                // Remove bird
                parrotObj.transform.position = new Vector3(2000, 0, 0);
                Thing parrot = parrotObj.GetComponent<Thing>();
                parrot.PlaySound();
            } else if (progressDone == 1) {
                GameObject cage = GameObject.Find("Cage");
                parrotObj.transform.position = cage.transform.position;
            }

        } else if (currentQuest == 4) {
            /*if (progressDone == 0) {
                // Remove water jug
                GameObject waterJugObj = GameObject.Find(GetQuest(currentQuest)[progressDone]);
                GameObject.Destroy(waterJugObj);
            } else */
            if (progressDone == 0) {
                // Water in the sink
                Thing sink = GameObject.Find(GetQuest(currentQuest)[progressDone]).GetComponent<Thing>();
                sink.PlaySound();
            } else if (progressDone == 1) {
                // Water the flower
                Thing flower = GameObject.Find(GetQuest(currentQuest)[progressDone]).GetComponent<Thing>();
                flower.PlaySound();
            }
        }
    }
}
