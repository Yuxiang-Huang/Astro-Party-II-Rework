import java.util.*;
import java.io.*;
import java.awt.*;
import java.lang.*;

public class Main {
  public static void main(String[] args) throws InterruptedException {
    for (int times = 17; times < 18; times ++){
      Screen s = new Screen();
      Color c;

      int B = 100;

      for (int i = 300; i <= 500; i ++){
        B -= 1;
        B = Math.max(0, B);
        c = new Color (0, 0, B);
        //System.out.println(c);
        s.drawLine(0, i, Screen.XRES-1, i, c);
        // Thread.sleep(100);
        // s.display();
      }

      c = new Color (255, 255, 255);

      for (int i = 0; i < 50; i ++){
        Point rand = new Point ((int) (Math.random() * Screen.XRES),
        (int) (Math.random() * Screen.YRES) + 300);
        s.drawLine(rand.x - 1, rand.y, rand.x + 1, rand.y, c);
        s.drawLine(rand.x, rand.y - 1, rand.x, rand.y + 1, c);
      }

      String name = "Background" + times;

      s.display();
      s.saveExtension(name + ".png");
    }
  }
}
