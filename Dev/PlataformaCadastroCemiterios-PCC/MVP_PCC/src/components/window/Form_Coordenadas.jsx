import React, { useState, useEffect } from 'react';
import { useSelector } from 'react-redux';
import { transform } from 'ol/proj'; // Para realizar a conversão entre sistemas de coordenadas
import proj4 from 'proj4';
import { register } from 'ol/proj/proj4';  
import PropTypes from 'prop-types'; 
import { Window as WindowDHX, Form as FormDHX, Tabbar as TabbarDHX, Menu as MenuDHX, ContextMenu as ContextMenuDHX, Tree as TreeDHX, Layout as LayoutDHX, Grid as GridDHX } from "dhx-suite";
import '../../../css/pcc.css';
import 'dhx-suite/codebase/suite.min.css';
import * as Actions from "../../constants/actions";
import ReactDOMServer from 'react-dom/server';
import { Icon } from '@fluentui/react';

// Registrar sistemas de coordenadas personalizados usando proj4
//proj4.defs('EPSG:3763', '+proj=utm +zone=29 +datum=WGS84 +units=m +no_defs'); // ETRS89 / PT-TM06
//proj4.defs('EPSG:27493', '+proj=tmerc +lat_0=39.68 +lon_0=-8.13 +k=1 +x_0=200000 +y_0=300000 +datum=Datum73 +units=m +no_defs'); // Datum 73

proj4.defs('EPSG:20790', '+proj=tmerc +lat_0=39.68 +lon_0=-8.13 +k=1 +x_0=180000 +y_0=200000 +datum=DatumLX +units=m +no_defs'); // Datum LX


proj4.defs("EPSG:3763","+proj=tmerc +lat_0=39.6682583333333 +lon_0=-8.13310833333333 +k=1 +x_0=0 +y_0=0 +ellps=GRS80 +towgs84=0,0,0,0,0,0,0 +units=m +no_defs +type=crs");
proj4.defs("EPSG:27493","+proj=tmerc +lat_0=39.6666666666667 +lon_0=-8.13190611111111 +k=1 +x_0=180.598 +y_0=-86.99 +ellps=intl +towgs84=-239.749,88.181,30.488,-0.263,-0.082,-1.211,2.229 +units=m +no_defs +type=crs");
proj4.defs("EPSG:3857","+proj=merc +a=6378137 +b=6378137 +lat_ts=0 +lon_0=0 +x_0=0 +y_0=0 +k=1 +units=m +nadgrids=@null +wktext +no_defs +type=crs");



register(proj4); // Registra proj4 em OpenLayers

 

const Form_Coordenadas = ({ coordenadas, srid }) => {
  const [convertedCoords, setConvertedCoords] = useState(null);

  // Função para realizar as conversões
  const convertCoordinates = () => {
    let etrs89 = [0,0];
    let dt73 = [0,0];
    let dtlx = [0,0];
    let wgs84 = [0,0];
    let web = [0,0];

    try{
      etrs89 = transform(coordenadas, `EPSG:${srid}`, 'EPSG:3763'); // ETRS89
    } catch(e){ etrs89 = [0,0]; }
    try{
      dt73 = transform(coordenadas, `EPSG:${srid}`, 'EPSG:27493'); // Datum 73
    } catch(e){ dt73 = [0,0]; }
    try{
      dtlx = transform(coordenadas, `EPSG:${srid}`, 'EPSG:20790'); // Datum LX
    } catch(e){ dtlx = [0,0]; }
    try{
      wgs84 = transform(coordenadas, `EPSG:${srid}`, 'EPSG:4326'); // WGS84
    } catch(e){ wgs84 = [0,0]; }
    try{
      web = transform(coordenadas, `EPSG:${srid}`, 'EPSG:3857'); // Web Mercator
    } catch(e){ web = [0,0]; }
    
    setConvertedCoords({
      etrs89: etrs89,// as [number, number],
      dt73: dt73,// as [number, number],
      dtlx: dtlx,// as [number, number],
      wgs84: wgs84,// as [number, number],
      web: web,// as [number, number]
    });
  };
  const fecharCoordenadas = () => {
    var aux={ type:'MAPA/HIDE_COORDENADAS', payload:''};
    //zoomToView(aux);
    viewer.dispatch(aux);
  };
  // useEffect para converter coordenadas automaticamente quando coordenadas ou srid mudarem
  useEffect(() => {
    if (coordenadas && srid) {
      convertCoordinates();
    }
  }, [coordenadas, srid]);

  return (
    <div style={{ textAlign: 'center' }}>
      {convertedCoords && (
        <table className="table table-condensed table-bordered">
          <tbody>
            <tr>
              <th colSpan={2}><small>ETRS89, PT-TM06 (EPSG:3763)</small></th>
            </tr>
            <tr>
              <td><small><strong>x: </strong>{convertedCoords.etrs89[0].toFixed(2)}</small></td>
              <td><small><strong>y: </strong>{convertedCoords.etrs89[1].toFixed(2)}</small></td>
            </tr>

            <tr>
              <th colSpan={2}><small>Hayford-Gauss, Datum 73, IGP (EPSG:27493)</small></th>
            </tr>
            <tr>
              <td><small><strong>x: </strong>{convertedCoords.dt73[0].toFixed(2)}</small></td>
              <td><small><strong>y: </strong>{convertedCoords.dt73[1].toFixed(2)}</small></td>
            </tr>

            <tr>
              <th colSpan={2}><small>Hayford-Gauss, Datum LX, Militar (EPSG:20790)</small></th>
            </tr>
            <tr>
              <td><small><strong>x: </strong>{convertedCoords.dtlx[0].toFixed(2)}</small></td>
              <td><small><strong>y: </strong>{convertedCoords.dtlx[1].toFixed(2)}</small></td>
            </tr>

            <tr>
              <th colSpan={2}><small>WGS84 (EPSG:4326)</small></th>
            </tr>
            <tr>
              <td colSpan={2}><small>Graus Decimais</small></td>
            </tr>
            <tr>
              <td><small><strong>lon: </strong>{convertedCoords.wgs84[0].toFixed(7)}</small></td>
              <td><small><strong>lat: </strong>{convertedCoords.wgs84[1].toFixed(7)}</small></td>
            </tr>

            <tr>
              <th colSpan={2}><small>Web Mercator (EPSG:3857)</small></th>
            </tr>
            <tr>
              <td><small><strong>x: </strong>{convertedCoords.web[0].toFixed(2)}</small></td>
              <td><small><strong>y: </strong>{convertedCoords.web[1].toFixed(2)}</small></td>
            </tr>
          </tbody>
        </table>
      )}
      <div style={{ marginTop: '10px', marginBottom: '10px' }}>
        <button className="btn btn-primary"  onClick={fecharCoordenadas}>Fechar</button>
      </div> 
    </div>
  );
};

export default Form_Coordenadas;
