/**
 * @author Koncsik Bendek Anndrás
 * Date: 2022. 12. 16.
 * Class name: LedControleer
 */
package hu.koncsik.ledcontroller.controller;

import hu.koncsik.ledcontroller.service.LedService;
import lombok.extern.slf4j.Slf4j;
import org.springframework.beans.factory.annotation.Autowired;
import org.springframework.http.MediaType;
import org.springframework.stereotype.Service;
import org.springframework.web.bind.annotation.GetMapping;
import org.springframework.web.bind.annotation.RestController;

@RestController("")
@Slf4j
public class LedController {
    private LedService ledService;

    @Autowired
    public void setLedService(LedService ledService) {
        this.ledService = ledService;
    }

    @GetMapping(value = "/on", produces = MediaType.APPLICATION_JSON_VALUE)
    public String on(){
        log.info("on");
        ledService.on();
        return "led: on";
    }

    @GetMapping(value = "/off", produces = MediaType.APPLICATION_JSON_VALUE)
    public String off(){
        log.info("off");
        ledService.off();
        return "led: off";
    }
}
