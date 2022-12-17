/**
 * @author Koncsik Bendek Anndrás
 * Date: 2022. 12. 17.
 * Class name: UsbService
 */
package hu.koncsik.ledcontroller.service;

import lombok.extern.slf4j.Slf4j;
import org.springframework.stereotype.Service;
import com.fazecast.jSerialComm.SerialPort;

import java.io.IOException;

@Service
@Slf4j
public class UsbService {


    private final SerialPort sp = SerialPort.getCommPort("arduino_uno_controller");
    public UsbService() {
        // default connection settings for Arduino
        sp.setComPortParameters(9600, 8, 1, 0);
        // block until bytes can be written
        sp.setComPortTimeouts(SerialPort.TIMEOUT_WRITE_BLOCKING, 0, 0);


    }
    private boolean closePort(){

        if (sp.closePort()) {
            log.info("Port closed!");
            return true;
        } else {
            log.error("Port flied closed!");
            return false;
        }
    }


    private boolean openPort(){
        if (sp.openPort()) {
            log.info("Port open!");
            return true;
        } else {
            log.error("Port flied open!");
            return false;
        }
    }
    public void testCommunication() throws IOException, InterruptedException {
        if (openPort()){
            for (Integer i = 0; i < 5; ++i) {
                sp.getOutputStream().write(i.byteValue());
                sp.getOutputStream().flush();
                log.info("Sent number: " + i);
                Thread.sleep(1000);
            }
            closePort();
        }



    }
}
