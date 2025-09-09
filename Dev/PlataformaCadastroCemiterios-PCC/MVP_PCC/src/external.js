
export function Teste(){
    const html = "<h3>dhtmlxWindow</h3><p>Here is a neat and flexible window system with fast initialization, convenient modes, and easy customization.</p><p>Inspect all the samples to discover each and every feature.</p><img style='display: block; width: 200px; height: 200px; margin-top: 20px; margin-left: auto; margin-right: auto' src='https://snippet.dhtmlx.com/codebase/data/common/img/01/developer-01.svg'>";
    const dhxWindow = new dhx.Window({
        width: 800,
        height: 520,
        title: 'Pretensão fora' ,//+ atendimentoId,
        //html: windowHtmlContainer,
        closable: true,
        movable: true,
        resizable: true,
        modal: false,
        header: true,
        footer: true,
    });
    dhxWindow.attachHTML(html);
    dhxWindow.footer.data.add({
        type: "button",
        view: "flat",
        size: "medium",
        color: "primary",
        value: "evento interno",
        id: "interno",
      });  
    dhxWindow.show();
    dhxWindow.footer.events.on("click", function(id,e){
        console.log(id);
        if (id==='interno'){
            viewer.dispatch({
                type: "GridATE/LIMPA",
                payload: {
                    atendimentoId: ""
              }});

            //dhxWindow.close();

            if (globalWindowReference) {
                globalWindowReference.hide();

                //globalWindowReference.events.fire("close",args);
            } 
        }
    });

}

  // Function to get the value of a URL parameter by name
  export function getUrlParameter(name) {
    name = name.replace(/[[]/, '\\[').replace(/[\]]/, '\\]');
    var regex = new RegExp('[\\?&]' + name + '=([^&#]*)');
    var results = regex.exec(location.search);
    return results === null ? '' : decodeURIComponent(results[1].replace(/\+/g, ' '));
}