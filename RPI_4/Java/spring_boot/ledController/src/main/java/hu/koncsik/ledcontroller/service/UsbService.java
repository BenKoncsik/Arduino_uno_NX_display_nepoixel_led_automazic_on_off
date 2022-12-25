/**
 * @author Koncsik Bendek Anndrás
 * Date: 2022. 12. 17.
 * Class name: UsbService
 */
package hu.koncsik.ledcontroller.service;

import lombok.extern.slf4j.Slf4j;

import org.springframework.stereotype.Service;
import com.fazecast.jSerialComm.SerialPort;

@Service
@Slf4j
public class UsbService {

    private SerialPort sp = null;

    public UsbService() {
        setUsb();
    }

    public boolean setUsb(){
        SerialPort serialPorts[] = SerialPort.getCommPorts();
        for (SerialPort sp:
                serialPorts) {
            if (sp.getPortDescription().equals("Arduino Uno")) this.sp = sp;
        }
        if (this.sp == null){
            log.error("No connection Arduino Uno");
            return false;
        }else {
            // default connection settings for Arduino
            sp.setComPortParameters(9600, Byte.SIZE, SerialPort.ONE_STOP_BIT, SerialPort.NO_PARITY);
            // block until bytes can be written
            sp.setComPortTimeouts(SerialPort.TIMEOUT_WRITE_BLOCKING, 0, 0);
            openPort();
            return true;
        }
    }

    public boolean closePort(){

        if (sp.closePort()) {
            log.info("Port closed!");
            return true;
        } else {
            log.error("Port flied closed!");
            return false;
        }
    }

    public boolean openPort(){
        if (sp.openPort()) {
            log.info("Port open!");
            return true;
        } else {
            log.error("Port flied open!");
            return false;
        }
    }

    public void on() {
            try {
                byte b = 49;
                sp.getOutputStream().write(b);
                Thread.sleep(SerialPort.TIMEOUT_WRITE_BLOCKING);
                sp.getOutputStream().flush();
                log.info("Sent: " + 1 + " byte: " + (byte) 1);
                Thread.sleep(SerialPort.TIMEOUT_WRITE_BLOCKING);
                if (sp.bytesAvailable() > 0){
                    log.info("Usb communication return: " + sp.getInputStream().read());
                }
            }catch (Exception e){
                log.error("Failed communication not found Arduino Uno!");
                log.error("Find Arduino uno: ");
                if (setUsb()) {
                    log.info("Successful solution!");
                    on();
                } else{
                    log.error("Big problem: " + e);
                }
            }
        }


    public void off() {
            try {
                byte b = 50;
                sp.getOutputStream().write(b);
                Thread.sleep(SerialPort.TIMEOUT_WRITE_BLOCKING);
                log.info("Sent: " + 2 + " byte: " + (byte) 2);
                Thread.sleep(SerialPort.TIMEOUT_WRITE_BLOCKING);
                sp.getOutputStream().flush();
                if (sp.bytesAvailable() > 0){
                    log.info("Usb communication retur: " + sp.getInputStream().read());
                }

            }catch (Exception e){
                log.error("Failed communication not found Arduino Uno!");
                log.error("Find Arduino uno: ");
                if (setUsb()) {
                    log.info("Successful solution!");
                    off();
                } else{
                    log.error("Big problem: " + e);
                }
            }
        }

    /**
     * Math: min value 502.56 max: value: 176181.81
     * only 1 byte: ((measurement + 492) [= 1000]) / 1000)
     * @return ((measurement - 492 ) * 1000)
     */

    public float measurement() {
        float mes = 0.0F;
        if (openPort()){
            try {
                byte b = 51;
                sp.getOutputStream().write(b);
                sp.getOutputStream().flush();
                Thread.sleep(1000);
                if (sp.bytesAvailable() > 0){
                   mes = sp.getInputStream().read();
                }
                log.info("Measurement: " + mes);
                mes = ((mes*1000)-482);
                log.info("Measurement: " + mes);
            }catch (Exception e){
                log.error("Failed communication not found Arduino Uno!");
                log.error("Find Arduino uno: ");
                if (setUsb()) {
                    log.info("Successful solution!");
                    measurement();
                } else{
                    log.error("Big problem: " + e);
                }
            }
        }
        return mes;
    }


    public boolean autoLed() {
        boolean autoLedB = true;
        try {
            byte b = 54;
            sp.getOutputStream().write(b);
            Thread.sleep(SerialPort.TIMEOUT_WRITE_BLOCKING);
            Thread.sleep(1000);
            if (sp.bytesAvailable() > 0){
                autoLedB = sp.getInputStream().read() != 0;
            }
            sp.getOutputStream().flush();
            log.info("auto led: " + autoLedB);
        }catch (Exception e){
            log.error("Failed communication not found Arduino Uno!");
            log.error("Find Arduino uno: ");
            if (setUsb()) {
                log.info("Successful solution!");
                autoLed();
            } else{
                log.error("Big problem: " + e);
            }
        }
        return autoLedB;
    }

    public int setBrightness(int level){
        if (level > 255) level = 255;
        if (level < 0) level = 0;
        try {
            byte b = 55;
            sp.getOutputStream().write(b);
            Thread.sleep(SerialPort.TIMEOUT_WRITE_BLOCKING);
            sp.getOutputStream().write(level);
            Thread.sleep(SerialPort.TIMEOUT_WRITE_BLOCKING);
            sp.getOutputStream().flush();
            if (sp.bytesAvailable() > 0){
                log.info("Usb communication level: " + sp.getInputStream().read());
            }
        }catch (Exception e){
            log.error("Failed communication not found Arduino Uno!");
            log.error("Find Arduino uno: ");
            if (setUsb()) {
                log.info("Successful solution!");
                setBrightness(level);
            } else{
                log.error("Big problem: " + e);
            }
        }
        return level;
    }

    public int[] setColor(int red,int green,int blue){
        if (red > 255) red = 255;
        if (red < 0) red = 0;
        if (green > 255) red = 255;
        if (green < 0) red = 0;
        if (blue > 255) red = 255;
        if (blue < 0) red = 0;
        int[] color = {red, green, blue};
        try {
            byte b = 56;
            sp.getOutputStream().write(b);
            Thread.sleep(SerialPort.TIMEOUT_WRITE_BLOCKING);
            for (int colorComponent: color) {
                sp.getOutputStream().write(colorComponent);
                Thread.sleep(SerialPort.TIMEOUT_WRITE_BLOCKING);
                sp.getOutputStream().flush();
                if (sp.bytesAvailable() > 0){
                    log.info("Usb communication color component: "+ colorComponent + " = " + sp.getInputStream().read());
                }
            }


            sp.getOutputStream().write(red);
            Thread.sleep(SerialPort.TIMEOUT_WRITE_BLOCKING);
            sp.getOutputStream().flush();
            if (sp.bytesAvailable() > 0){
                log.info("Usb communication level: " + sp.getInputStream().read());
            }
        }catch (Exception e){
            log.error("Failed communication not found Arduino Uno!");
            log.error("Find Arduino uno: ");
            if (setUsb()) {
                log.info("Successful solution!");
                setBrightness(red);
            } else{
                log.error("Big problem: " + e);
            }
        }
        return color;
    }

    public int setColorBrightness(int level){
        if (level > 100) level = 100;
        if (level < 0) level = 0;
        try {
            byte b = 57;
            sp.getOutputStream().write(b);
            Thread.sleep(SerialPort.TIMEOUT_WRITE_BLOCKING);
            sp.getOutputStream().write(level);
            Thread.sleep(SerialPort.TIMEOUT_WRITE_BLOCKING);
            sp.getOutputStream().flush();
            if (sp.bytesAvailable() > 0){
                log.info("Usb communication level: " + sp.getInputStream().read());
            }
        }catch (Exception e){
            log.error("Failed communication not found Arduino Uno!");
            log.error("Find Arduino uno: ");
            if (setUsb()) {
                log.info("Successful solution!");
                setBrightness(level);
            } else{
                log.error("Big problem: " + e);
            }
        }
        return level;
    }


    public int setOnAutoLevel(int level){
        level = 255-level;
        if (level > 255) level = 255;
        if (level < 0) level = 0;
        try {
            byte b = 58;
            sp.getOutputStream().write(b);
            Thread.sleep(SerialPort.TIMEOUT_WRITE_BLOCKING);
            sp.getOutputStream().write(level);
            Thread.sleep(SerialPort.TIMEOUT_WRITE_BLOCKING);
            sp.getOutputStream().flush();
            if (sp.bytesAvailable() > 0){
                int response = sp.getInputStream().read();
                log.info("Usb communication set on auto led is on: " + response + "-->" + ((response * 690.9090588) - 502.56));

            }
        }catch (Exception e){
            log.error("Failed communication not found Arduino Uno!");
            log.error("Find Arduino uno: ");
            if (setUsb()) {
                log.info("Successful solution!");
                setOnAutoLevel(level);
            } else{
                log.error("Big problem: " + e);
            }
        }
        return level;
    }
}
