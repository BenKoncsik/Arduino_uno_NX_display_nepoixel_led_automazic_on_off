/**
 * @author Koncsik Bendek Anndrás
 * Date: 2022. 12. 16.
 * Class name: LedService
 */
package hu.koncsik.ledcontroller.service;

import com.pi4j.Pi4J;
import com.pi4j.context.Context;
import hu.koncsik.ledcontroller.component.LedStrip;
import org.springframework.stereotype.Service;
import lombok.extern.slf4j.Slf4j;
@Service
@Slf4j
public class LedService {
    final Context pi4j = Pi4J.newAutoContext();
    final int pixels = 30;
    final LedStrip ledStrip = new LedStrip(pi4j, pixels, 0.5);
    public void on(){
        ledStrip.render();
    }

    public void off() {
        ledStrip.allOff();
    }
}
