import * as React from "react";   
import { useReduxDispatch } from "mapguide-react-layout/lib/components/map-providers/context";
import { tr } from "mapguide-react-layout/lib/api/i18n";
import * as olProj from "ol/proj";
import { Callout, Intent, Card } from '@blueprintjs/core';
import { useViewerLocale, useCurrentMouseCoordinates, useActiveMapName } from 'mapguide-react-layout/lib/containers/hooks';
import { useActiveMapProjection } from 'mapguide-react-layout/lib/containers/hooks-mapguide'; 

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
            <h4 style={{color: "#fff" }} className="bp3-heading">{/*tr("COORDTRACKER", locale)*/}</h4>
            {aProjections.map(p => {
                let x = NaN;
                let y = NaN;
                if (mouse && proj) {
                    try {
                        [x, y] = olProj.transform(mouse, proj, p);
                    } catch (e) { 
                    }
                }
                return <Card key={p} style={{ marginBottom: 10 }}>
                    <h5 className="bp3-heading"><a  style={{color: "#fff" }} href="#">{p}</a></h5>
                    <p><strong>X:</strong> {x}</p>
                    <p><strong>Y:</strong> {y}</p>
                </Card>;
            })}
        </div>;
    } else {
        return <Callout intent={Intent.DANGER} title={tr("ERROR", locale)} icon="error">
            {tr("COORDTRACKER_NO_PROJECTIONS", locale)}
        </Callout>;
    }
}