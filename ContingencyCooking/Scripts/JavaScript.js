alert(message){

    message = 'Hello';

};


class MyClass {

}

do {

} while (true);


//basic syntax?
(function (){
    if (true) {

    }
    else {

    }
});

for (var i in o) {

}

for (var i = length - 1; i >= 0; i--) {

}

function myfunction() {

}

(function (args) {



})(args);

switch (switch_on) {

    default:
        console.log("Hello");
}


//AJAX snippets
//Behavior
Type.registerNamespace("mynamespace");

mynamespace.mycontrol = function (element) {
    mynamespace.mycontrol.initializeBase(this, [element]);
}

mynamespace.mycontrol.prototype = {
    initialize: function () {
        mynamespace.mycontrol.callBaseMethod(this, 'initialize');
    },
    dispose: function () {
        mynamespace.mycontrol.callBaseMethod(this, 'dispose');
    }
}

mynamespace.mycontrol.registerClass('mynamespace.mycontrol', Sys.UI.Behavior);


//Control
Type.registerNamespace("mynamespace");

mynamespace.mycontrol = function (element) {
    mynamespace.mycontrol.initializeBase(this, [element]);
}

mynamespace.mycontrol.prototype = {
    initialize: function () {
        mynamespace.mycontrol.callBaseMethod(this, 'initialize');
    },
    dispose: function () {
        mynamespace.mycontrol.callBaseMethod(this, 'dispose');
    }
}

mynamespace.mycontrol.registerClass('mynamespace.mycontrol', Sys.UI.Control);


//Create
$create(Sys.Component, {}, {}, {}, null)

