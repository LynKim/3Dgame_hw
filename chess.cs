using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class chess : MonoBehaviour
{
    int turn, counter;
    int[,] board = new int[3, 3];

    // Start is called before the first frame update
    void Start()
    {
        init();        
    }

    void init(){
        turn = 1;
        counter = 0;
        for(int i = 0; i < 3; i++){
            for(int j = 0; j < 3; j++){
                board[i, j] = 0;
            }
        }
    }

    int check(){
        for(int i = 0; i < 3; i++){
            if(board[0, i] == board[1, i] && board[1, i] == board[2, i]){
                return board[0, i];
            }
            if(board[i, 0] == board[i, 1] && board[i, 1] == board[i, 2]){
                return board[i, 0];
            }
        }

        if((board[0, 0] == board[1, 1] && board[1, 1] == board[2, 2]) || (board[0, 2] == board[1, 1] && board[1, 1] == board[2, 0])){
            return board[1, 1];
        }

        if(counter == 9){
            return 3;
        }

        return 0;
    }

    void OnGUI(){
        if(GUI.Button(new Rect(220, 100, 100, 50),"Restart")){
            init();
        }

        int result = check();
        if(result == 1){
            GUI.Label(new Rect(220, 50, 100, 50), "Player1 wins!");
        }
        else if(result == 2){
            GUI.Label(new Rect(220, 50, 100, 50), "Player2 wins!");
        }
        else if(result == 3){
            GUI.Label(new Rect(220, 50, 200, 50), "No winner, let's play again.");
        }

        for(int i = 0; i < 3; i++){
            for(int j = 0; j < 3; j++){
                if(board[i, j] == 1){
                    GUI.Button(new Rect(50+i*50, 20+j*50, 50, 50), "O");
                }
                if(board[i, j] == 2){
                    GUI.Button(new Rect(50+i*50, 20+j*50, 50, 50), "X");
                }
                if(GUI.Button(new Rect(50+i*50, 20+j*50, 50, 50), "")){
                    if(result == 0){
                        board[i, j] = turn;
                        counter++;
                        if(turn == 1){
                            turn = 2;
                        }
                        else{
                            turn = 1;
                        }
                    }
                }
            }
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}


