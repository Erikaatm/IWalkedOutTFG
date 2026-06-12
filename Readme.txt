Echos of You
Videojuego desarrollado como proyecto final del Ciclo Formativo de Grado Superior de Programación de Videojuegos.

Descripción
Echos of You es un videojuego 2D top-down con estética pixel art en el que el jugador explora distintas habitaciones ambientadas en entornos cotidianos, recoge objetos y los coloca en zonas de entrega específicas. Cada decisión influye en un sistema de karma dividido en tres ejes —Caos, Neutral y Perfección— que determina el aspecto del personaje y el desenlace narrativo final.

Autores

Erika Toledano Morgádez
David Muñoz Flores

Tutor: Jesús Rodríguez

Requisitos

Unity 6000.3.8f1 (Unity 6.3 LTS)
Visual Studio 2022 (o Visual Studio Code)

Paquetes necesarios (importar desde Package Manager)

Cinemachine — seguimiento de cámara del jugador
TextMeshPro — renderizado de texto en UI


Cómo abrir el proyecto

Clona o descarga el repositorio
Abre Unity Hub
Haz clic en Add project from disk y selecciona la carpeta del proyecto
Asegúrate de tener instalada la versión Unity 6000.3.8f1
Abre el proyecto


Cómo ejecutar

Para un correcto flujo del juego, ejecutar siempre desde la escena MainMenu.


En el panel Project, navega a Assets/Scenes/
Abre la escena MainMenu
Dale a Play


Controles
AcciónTeclaMoverseW A S DRecoger objeto / Interactuar / Activar portalESeleccionar slot del hotbar1 2 3 4Abrir menú (inventario, mapa, ajustes...)TAB

Estructura de escenas
EscenaDescripciónMainMenuMenú principal del juegoIntroSceneEscena inicial de selección de gema de karma (compañera)SampleSceneEscena principal de juego con todas las habitacionesEscenaFinalTexto narrativo final según karma acumuladoCreditosPantalla de créditos

Notas para el evaluador

El juego guarda el progreso automáticamente en Application.persistentDataPath/saveData.json
Al completar el juego, el save se reinicia automáticamente para permitir una nueva partida
El personaje cambia de sprite dinámicamente según el karma dominante durante la partida
Los NPCs del mapa proporcionan pistas sobre la colocación de objetos y la historia


Recursos externos utilizados

Asset packs de tilesets y sprites obtenidos de internet (ver créditos en juego)
Clip de audio de pasos: freesound.org — licencia Creative Commons
Personaje principal: diseño e implementación propios