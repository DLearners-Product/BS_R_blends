using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[SerializeField]
public class EnablerCmtsDB
{
    public string welcome;
    public string brain_gym_brain_buttons;
    public string introduction;
    public string image_introduction;
    public string sorting_activity;
    public string matching_activity;
    public string phonological_awareness;
    public string brain_gym_tiger_pose;
    public string word_search;
    public string identify_the_R_blends;
    public string story_time;
    public string goodbye_song;

    public EnablerCmtsDB()
    {
        welcome = Main_Blended.OBJ_main_blended.enablerComments[0];
        brain_gym_brain_buttons = Main_Blended.OBJ_main_blended.enablerComments[1];
        introduction = Main_Blended.OBJ_main_blended.enablerComments[2];
        image_introduction = Main_Blended.OBJ_main_blended.enablerComments[3];
        sorting_activity = Main_Blended.OBJ_main_blended.enablerComments[4];
        matching_activity = Main_Blended.OBJ_main_blended.enablerComments[5];
        phonological_awareness = Main_Blended.OBJ_main_blended.enablerComments[6];
        brain_gym_tiger_pose = Main_Blended.OBJ_main_blended.enablerComments[7];
        word_search = Main_Blended.OBJ_main_blended.enablerComments[8];
        identify_the_R_blends = Main_Blended.OBJ_main_blended.enablerComments[9];
        story_time = Main_Blended.OBJ_main_blended.enablerComments[10];
        goodbye_song = Main_Blended.OBJ_main_blended.enablerComments[11];
    }
}