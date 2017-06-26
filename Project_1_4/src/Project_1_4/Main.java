package Project_1_4;
import java.security.InvalidKeyException;
import java.security.NoSuchAlgorithmException;
import java.security.SignatureException;
import java.util.*;
import javax.crypto.Mac;
import javax.crypto.spec.SecretKeySpec;

public class Main {
    static int SIZE = 3;
    public static List<String> list_Of_Names = new ArrayList<>(Arrays.asList("Rock", "Scissors", "Paper"));

    public static void print_Choice(){
        for(int i = 0; i < SIZE; i++){
            System.out.println(i + ". " + list_Of_Names.get(i));
        }
    }

    public static void main(String[] args) throws NoSuchAlgorithmException, SignatureException, InvalidKeyException {
        //Fill list of items and show them on screen
        print_Choice();

        //Generating random number for encoding PC choice
        int encode_Number = 1000 + (int)(Math.random()*8999);
        int choice_PC = (int)(Math.random()*SIZE);

        //Encoding a SHA-1 string and showing it to the user
        String hmac_String = HMAC.calculateHMAC(list_Of_Names.get(choice_PC), String.valueOf(encode_Number));
        System.out.println("Encoded choice of PC: " + hmac_String);

        //Entering the number of user
        System.out.println("Enter your turn: ");
        Scanner input = new Scanner(System.in);
        int choice_of_User = input.nextInt();

        //Resolving the winner
        int def = (SIZE + choice_of_User - choice_PC) % SIZE;
        if(def == 0)
            System.out.println("DRAW!!!!");
        else if(def % 2 == 0)
            System.out.println("YOU WIN!!!!!");
        else System.out.println("YOU LOSE!!!!");

        System.out.println("The choice of PC is: " + list_Of_Names.get(choice_PC));
        System.out.println("The key is: "+ encode_Number);
    }
}
