import * as Actions from "../constants/actions";
import { v4 as uuidv4 } from 'uuid';
export const setConstrucoesSelection = async (apiEndpointSIG, authtoken, activeMapName, mapaDef, sessionId, aux_construcaoid,  centro_x, centro_y, escala) => {					 
    
    const jwtToken = authtoken;
    const url = apiEndpointSIG + 'MapaConstrucoesSetSelection';//ok

    const response = await fetch(url, {
      method: 'POST',
      body: JSON.stringify({
          mapa: activeMapName,
          mapadef: mapaDef,
          sessionid: sessionId,             
          viewer: 'false',
          construcaoid: aux_construcaoid, 
          cx: centro_x.toString(),
          cy: centro_y.toString(),
          escala: escala.toString(),
        }),
      headers: {
          'Content-Type': 'application/json',
          'Authorization': `Bearer ${jwtToken}`
        },
    });

    if (response.ok) { 
      const xml = await response.text();  
      if (viewer_interface ==null){
        viewer_interface = GetViewerInterface(); 
      } 
      viewer_interface.setSelectionXml(xml);  

      var state = viewer.getState();
      const args = state.config;
      var NomeMapa = state.config.activeMapName;
      const uid = uuidv4();

       
      setTimeout(() => {
 
				var estado1 = viewer.getState();
				var activeMapName1 = estado1.config.activeMapName;
			
				viewer.dispatch({
					type: 'Map/SET_VIEW',
					payload: { mapName: activeMapName1, view: { x: centro_x.toString(), y: centro_y.toString(), scale: escala.toString() } }
				});
			}, 1000);  // 2000 milissegundos = 2 segundos
      
    } else {
      const errorMessage = await response.text();
      console.error('error: ' + errorMessage);
      this.setState({ errorMessage: errorMessage });
    } 
  };  



export const decodeHtmlEntities = (input) => {
  if (typeof input !== 'string') {
    return input; // Ou lançar um erro, dependendo do que fizer sentido para sua aplicação
  }

  return input
    .replace(/&amp;/g, '&')
    .replace(/&lt;/g, '<')
    .replace(/&gt;/g, '>')
    .replace(/&quot;/g, '"')
    .replace(/&#039;/g, "'") // ou &apos;
    .replace(/&apos;/g, "'") // Adicionando tratamento para &apos;
    .replace(/&#x([0-9a-fA-F]+);/g, (_, hex) => String.fromCharCode(parseInt(hex, 16)))
    .replace(/&#(\d+);/g, (_, dec) => String.fromCharCode(dec)); // Adicionando tratamento para entidades decimais
};

export const encodeHtmlEntities = (input) => {
  // Verifica se a entrada é uma string
  if (typeof input !== 'string') {
    console.warn("encodeHtmlEntities: A entrada não é uma string, retornando a entrada original.");
    return input; // Ou lançar um erro, dependendo do que fizer sentido para sua aplicação
  }

  // É crucial substituir '&' primeiro, para não escapar os '&' das outras entidades.
  return input
    .replace(/&/g, '&amp;')   // Ampersand
    .replace(/</g, '&lt;')    // Less than (menor que)
    .replace(/>/g, '&gt;')    // Greater than (maior que)
    .replace(/"/g, '&quot;')  // Aspas duplas
    .replace(/'/g, '&apos;'); // Apóstrofo (ou plica). &apos; é padrão em XML/XHTML e HTML5.
                               // Alternativamente, poderia usar &#039; ou &#x27;
};

export const processData = (data)  => {
 
    if (typeof data === 'string') {
      let parsedData = JSON.parse(data);
      parsedData.forEach(item => {
        processItem(item);
      });
      data = JSON.stringify(parsedData);
    } else {
      if (Array.isArray(data)) {
        data.forEach(item => {
            processItem(item);
        });
    }
  }

  return data;
}
export const  processItem = (item) => {
  if (item && typeof item === 'object') {
      if (item.value && typeof item.value === 'string') {
          item.value = decodeHtmlEntities(item.value);
      }
      if (item.items && Array.isArray(item.items)) {
          item.items.forEach(nestedItem => {
              processItem(nestedItem); // Recursão para itens aninhados
          });
      }
  }
}


export const convertDateEdicao2Storage = (dateString) =>{
  // Divide a string de data pelo caractere '/'
  const parts = dateString.split('/');

  // As partes estarão na ordem: [dia, mês, ano]
  const dia = parts[0];
  const mes = parts[1];
  const ano = parts[2];

  // Reordena as partes e junta-as numa nova string
  // O formato é 'AAAAMMDD'
  return `${ano}${mes}${dia}`;
}
export const convertDateStorage2Edicao = (dateString) =>{
  // A string de data deve ter 8 caracteres (AAAAMMDD)
  if (dateString.length !== 8) {
    return 'Formato de data inválido';
  }

  // Extrai as partes da data da string
  const ano = dateString.substring(0, 4);
  const mes = dateString.substring(4, 6);
  const dia = dateString.substring(6, 8);

  // Junta as partes no formato 'DD/MM/AAAA'
  return `${dia}/${mes}/${ano}`;
}