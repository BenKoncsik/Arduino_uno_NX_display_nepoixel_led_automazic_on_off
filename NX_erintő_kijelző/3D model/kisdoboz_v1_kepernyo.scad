 module prism(l, w, h){
     polyhedron(
               points=[[0,0,0], [l,0,0], [l,w,0], [0,w,0], [0,w,h], [l,w,h]],
              faces=[[0,1,2,3],[5,4,3,2],[0,4,5,1],[0,3,4],[5,2,1]]
              );
}


cube([130, 80, 2]);
cube([2, 80, 80]);


translate([128, 0, 0]) cube([2, 80, 80]);
//eleje
difference(){
cube([130, 2, 80]);
translate([14,0,65]) cube([110, 2, 15]);
}

//teteje
difference(){
translate([0, 0, 78]) cube([130, 80, 2]);
translate([14, 0, 78]) cube([110, 15, 2]);
}

//elso kis biszbasz
translate([14, -10, 50]) cube([110, 12, 3]);
translate([14, -10, 50]) cube([110, 2, 5]);
//hatulja
difference(){
translate([0, 78, 0]) cube([130, 2, 80]);
translate([55, 78, 10]) cube([20, 2, 10]);
}
 
      
