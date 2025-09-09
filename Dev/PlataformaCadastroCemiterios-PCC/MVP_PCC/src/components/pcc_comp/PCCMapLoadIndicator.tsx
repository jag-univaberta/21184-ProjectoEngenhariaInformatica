import * as React from "react"; 

/**
 * The positioning of the map load indicator
 */
export type PCCMapLoadIndicatorPositioning = "top" | "bottom" | "center";

export interface IPCCMapLoadIndicatorProps {
    isBusy: boolean; 
    /*position: PCCMapLoadIndicatorPositioning;*/
}

export const PCCMapLoadIndicator = (props: IPCCMapLoadIndicatorProps) => {
    const { isBusy } = props;
    let visibility: "visible" | "hidden" = "visible";
    //console.log(isBusy);
    if (!isBusy) {
        visibility = "hidden"; 
    }
    let width = "460px";/*25%";*/
    const style: React.CSSProperties = {
        position: "absolute",
        zIndex: 9999,
        visibility: visibility,
        left: 0,
        bottom: 0,
        height: 15,
        width: width, 
        transition: "width 250ms",
        backgroundColor:"#0f6cbd"
    };
     
    return <div className={`map-load-indicator`} style={style}> A aguardar por informação ...</div>;
}