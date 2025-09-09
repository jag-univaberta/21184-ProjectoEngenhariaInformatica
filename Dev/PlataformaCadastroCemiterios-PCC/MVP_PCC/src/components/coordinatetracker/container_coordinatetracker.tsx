import * as React from "react";   
import { useReduxDispatch } from "mapguide-react-layout/lib/components/map-providers/context";
import { tr } from "mapguide-react-layout/lib/api/i18n";
import * as olProj from "ol/proj";
import { Callout, Intent, Card } from '@blueprintjs/core';
import { useViewerLocale, useCurrentMouseCoordinates, useActiveMapName } from 'mapguide-react-layout/lib/containers/hooks';
import { useActiveMapProjection } from 'mapguide-react-layout/lib/containers/hooks-mapguide'; 
import { none } from "ol/centerconstraint";
import { Padding } from "@mui/icons-material";

export interface IContainerCoordinateTrackerContainerProps {
    projections: string | string[];
}

export const ContainerCoordinateTrackerContainer = (props: IContainerCoordinateTrackerContainerProps) => {
    const { projections } = props;
    const aProjections = Array.isArray(projections) ? projections : [projections];
    const locale = useViewerLocale();
    const mouse = useCurrentMouseCoordinates();
    const proj = useActiveMapProjection();
    if (aProjections && aProjections.length) {
        return <div style={{ margin: 8 }}> 
            
            {aProjections.map(p => {
                let x = NaN;
                let y = NaN;
                if (mouse && proj) {
                    try {
                        [x, y] = olProj.transform(mouse, proj, p);
                    } catch (e) { 
                    }
                }
                let descricao='';
                switch (p) {
                    case "EPSG:3763":
                        descricao='ETRS89, PT-TM06 (EPSG:3763)';
                        break;  
                    case "EPSG:27493":
                        descricao='Hayford-Gauss, Datum 73, IGP (EPSG:27493)';
                        break; 
                    case "EPSG:20790":
                        descricao='Hayford-Gauss, Datum LX, Militar (EPSG:20790)';
                        break;    
                    case "EPSG:4326":
                        descricao='WGS84 (EPSG:4326)';
                        break;  
                    case "EPSG:3857":
                        descricao='Web Mercator (EPSG:3857)';
                        break;      
                    default:
                        descricao='';
                        break;   
                }

                return <Card key={p} style={{ marginBottom: 2, padding: 0}}>
                    <table className="table table-condensed table-bordered" style={{ width: '100%' }}> 
                    <tbody>
                    <tr>
                    <th colSpan={2}><small>{descricao}</small></th>
                    </tr>
                    <tr>
                    <td style={{ width: '50%' }}><small><strong>X: </strong>{x.toFixed(7)}</small></td>
                    <td><small><strong>Y: </strong>{y.toFixed(7)}</small></td>
                    </tr>
                    </tbody>
                     </table>
                </Card>;
            })}
            
        </div>;
    } else {
        return <Callout intent={Intent.DANGER} title={tr("ERROR", locale)} icon="error">
            {tr("COORDTRACKER_NO_PROJECTIONS", locale)}
        </Callout>;
    }
}