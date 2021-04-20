using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace dataObj{
    public class dataPlayer
    {
        string namePlayer;
        int blood;
        int killed;
        int died;
        dataPlayer();
        dataPlayer(string namePlayer,int blood,int killed,int died);
        public void setDataAll(string namePlayer, int blood, int killed, int died){
            this.namePlayer = namePlayer;
            this.blood = blood;
            this.killed = killed;
            this.died = died;
        }
        public void setNamePlayer(string name){this.namePlayer = name;}
        public void setBlood(int blood){this.blood = blood;}
        public void setKilled(int killed){ this.killed = killed; }
        public void setDied(int died){this.died = died;}
    

        public string getNamePlayer(){ return this.namePlayer; }
        public int getBlood() { return this.blood; }
        public int getKilled() { return this.killed; }
        public int getDied(){ return this.died; }
    }
}
