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
//
//    switch(incomingByte){
//        case "1":
//            led_on();
//            break;
//        case "2":
//            led_off();
//            break;
//        case "3":
//            float mes = measurement();
//            Serial.print(mes);
//            break;
//        default:
//            led_first_on();
//    }

//     if (incomingString.equals("5010")) {
//        Serial.println(" off");
//        led_off();
//    }
//    if (incomingString.equals("5110")) {
//        float mes = measurement();
//        Serial.print(" ");
//        Serial.println(mes);
//    }
    private SerialPort sp = null;

    public UsbService() {
        SerialPort serialPorts[] = SerialPort.getCommPorts();
        for (SerialPort sp:
                serialPorts) {
            if (sp.getPortDescription().equals("Arduino Uno")) this.sp = sp;
        }
        if (this.sp == null) log.error("No connection Arduino Uno");
        // default connection settings for Arduino
        sp.setComPortParameters(9600, Byte.SIZE, SerialPort.ONE_STOP_BIT, SerialPort.NO_PARITY);
        // block until bytes can be written
        sp.setComPortTimeouts(SerialPort.TIMEOUT_WRITE_BLOCKING, 0, 0);
        openPort();

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
//                log.info(String.valueOf(sp.getInputStream().read()));
            }catch (Exception e){
                log.error("Failed communication: " + e);
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
            }catch (Exception e){
                log.error("Failed communication: " + e);
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
                log.error("Failed communication: " + e);
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
        }catch (Exception e){
            log.error("Failed communication: " + e);
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
            sp.getOutputStream().flush();
            Thread.sleep(500);
            sp.getOutputStream().write(level);
            Thread.sleep(SerialPort.TIMEOUT_WRITE_BLOCKING);
            sp.getOutputStream().flush();
            if (sp.bytesAvailable() > 0){
                log.info("Usb communication level: " + sp.getInputStream().read());
            }
        }catch (Exception e){
            log.error("Failed communication: " + e);
        }
        return level;
    }

}
