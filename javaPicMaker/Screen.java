import java.awt.image.*;
import java.awt.*;
import java.io.*;
import javax.imageio.*;
import javax.swing.*;

public class Screen {

  public static final int XRES = 500;
  public static final int YRES = 200;
  public static final int MAX_COLOR = 255;
  public static final Color DEFAULT_COLOR = new Color(0, 0, 0);

  private int width;
  private int height;

  private BufferedImage img;

  public Screen() {
    width = XRES;
    height = YRES;

    img = new BufferedImage(width, height, BufferedImage.TYPE_INT_RGB);
    clearScreen();
  }//constructor

  public void clearScreen() {

    Graphics2D g = img.createGraphics();
    g.setColor(DEFAULT_COLOR);
    g.fillRect(0, 0, img.getWidth(), img.getHeight());
    g.dispose();

  }//clearScreen

    /*=============================
      PUT YOUR LINE ALGORITHM CODE HERE
      ===========================*/
  public void drawLine(int x0, int y0, int x1, int y1, Color c) {
    if (x1 < x0){
      int temp = x0;
      x0 = x1;
      x1 = temp;

      temp = y0;
      y0 = y1;
      y1 = temp;
    }

    int dx = x1 - x0;
    int dy = y1 - y0;

    if (dx == 0){
      if (y0 > y1){
        int temp = y0;
        y0 = y1;
        y1 = temp;
      }
      int y = y0;
      while (y <= y1){
        plot(c, x0, y);
        y++;
      }
    }

    else if (dy == 0){
      int x = x0;
      while (x <= x1){
        plot(c, x, y0);
        x++;
      }
    }

    //octant1
    else if (dy > 0 && dy <= dx){
      int A = -dy;
      int B = dx;
      int d = 2 * A + B;
      int x = x0;
      int y = y0;
      while (x < x1){
        plot(c, x, y);
        if (d < 0){
          y ++;
          d += 2 * B;
        }
        x ++;
        d += 2 * A;
      }
    }

    //octant2
    else if (dy > 0 && dy > dx){
      int A = -dy;
      int B = dx;
      int d = A + 2 * B;
      int x = x0;
      int y = y0;
      while (y < y1){
        plot(c, x, y);
        if (d > 0){
          x ++;
          d += 2 * A;
        }
        y ++;
        d += 2 * B;
      }
    }

    //octant7
    else if (dy < 0 && Math.abs(dy) <= dx){
      int A = -dy;
      int B = dx;
      int d = 2 * A - B;
      int x = x0;
      int y = y0;
      while (x < x1){
        plot(c, x, y);
        if (d > 0){
          y --;
          d -= 2 * B;
        }
        x ++;
        d += 2 * A;
      }
    }

    //octant8
    else if (dy < 0 && Math.abs(dy) > dx){
      int A = -dy;
      int B = dx;
      int d = A - 2 * B;
      int x = x0;
      int y = y0;
      while (y > y1){
        plot(c, x, y);
        if (d < 0){
          x ++;
          d += 2 * A;
        }
        y --;
        d -= 2 * B;
      }
    }
  }//drawLine

  public void plot(Color c, int x, int y) {
    int newy = width - 1 - y;
    if (x >= 0 && x < width && newy >= 0 && newy < height) {
      img.setRGB(x, newy, c.getRGB());
    }
  }//plot

  public void savePpm(String filename) {
    String ppmFile = "P3\n";
    ppmFile+= width + " " + height + "\n";
    ppmFile+= MAX_COLOR + "\n";

    //int[] raster = img.getRGB(0, 0, img.getWidth(), img.getHeight(), null, 0, 1);
    for (int y=0; y < height; y++) {
      for (int x=0; x<width; x++) {
        Color c = new Color(img.getRGB(x, y));
        ppmFile+= c.getRed() + " ";
        ppmFile+= c.getGreen() + " ";
        ppmFile+= c.getBlue() + " ";
      }
      ppmFile+= "\n";
    }
    try {
      FileWriter ppmWriter = new FileWriter(filename);
      ppmWriter.write(ppmFile);
      ppmWriter.close();
    }
    catch(IOException e) {
      System.out.println("Unable to write to file");
      e.printStackTrace();
    }
  }//savePpm

  public void saveExtension(String filename) {
    String ext = filename.split("\\.")[1];
    boolean goodType = false;
    for (String typ : ImageIO.getWriterFormatNames()) {
      if (ext.equals(typ)) {
        goodType = true;
        break;
      }
    }//type check
    if ( !goodType ) {
      System.out.println("Bad File Extension: " + ext);
      return;
    }//bad extension
    try {
      File outputfile = new File(filename);
      ImageIO.write(img, ext, outputfile);
    }
    catch (IOException e) {
      System.out.println("Unable to write to file");
      e.printStackTrace();
    }

  }//saveExtension

  public void display() {
    JFrame frame = new JFrame();
    frame.setDefaultCloseOperation(WindowConstants.EXIT_ON_CLOSE);
    frame.setSize(img.getWidth(), img.getHeight() + 20);

    ColorModel colorModel = img.getColorModel();
    WritableRaster raster = img.copyData(null);
    boolean isAlphaPremultiplied = colorModel.isAlphaPremultiplied();
    BufferedImage cpy = new BufferedImage(colorModel, raster, isAlphaPremultiplied, null);

    JPanel pane = new JPanel() {
        @Override
        protected void paintComponent(Graphics g) {
          super.paintComponent(g);
          g.drawImage(cpy, 0, 0, null);
        }
      };

    frame.add(pane);
    frame.setVisible(true);
  }//display

}//class Screen
