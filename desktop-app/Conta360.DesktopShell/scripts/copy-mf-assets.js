const fs = require('fs-extra');
const path = require('path');
const rimraf = require('rimraf'); // Para limpiar directorios

const rootDir = path.join(__dirname, '..', '..'); // Apunta a la raíz del monorepo
const rootConfigOutDir = path.join(rootDir, 'src', 'microfrontends', 'root-config', 'out');
const remoteApps = ['dashboard-app', 'analisis-app', 'pgc-app', 'shared-components'];
// const remoteApps = ['dashboard-app', 'analisis-app', 'pgc-app', 'shared-components'];

console.log('Iniciando copia de assets de Module Federation para Tauri...');

async function copyMfAssets() {
  try {
    // 1. Limpiar directorio de chunks viejos de remotes
    const staticChunksDir = path.join(rootConfigOutDir, '_next', 'static', 'chunks');
    console.log(`Limpiando: ${staticChunksDir}/remoteEntry*.js`);
    await rimraf(path.join(staticChunksDir, 'remoteEntry*.js'));
    console.log('Limpieza de chunks de remotes completada.');

    // 2. Copiar remoteEntry.js y otros assets de remotes
    for (const app of remoteApps) {
      const remoteOutDir = path.join(rootDir, 'src', 'microfrontends', app, 'out');
      const remoteEntryPath = path.join(remoteOutDir, '_next', 'static', 'chunks', 'remoteEntry.js');
      const destinationPath = path.join(staticChunksDir, `${app}-remoteEntry.js`); // Renombra para evitar conflictos

      if (await fs.pathExists(remoteEntryPath)) {
        await fs.copy(remoteEntryPath, destinationPath);
        console.log(`Copiado: ${remoteEntryPath} -> ${destinationPath}`);
      } else {
        console.warn(`Advertencia: remoteEntry.js no encontrado para ${app} en ${remoteEntryPath}`);
      }

      // Copiar cualquier otro asset estático si Module Federation los refiriere
      // Esto es una simplificación. En un caso real, podría necesitar un script más inteligente
      // para copiar todos los assets necesarios referenciados por los remotes.
      const remoteStaticDir = path.join(remoteOutDir, '_next', 'static');
      if (await fs.pathExists(remoteStaticDir)) {
          const files = await fs.readdir(remoteStaticDir);
          for (const file of files) {
              // Copiar solo archivos relevantes, no recursivo en todas las carpetas, solo directo en 'static'
              if (file.endsWith('.js') || file.endsWith('.css')) {
                  const src = path.join(remoteStaticDir, file);
                  const dest = path.join(staticChunksDir, `${app}-${file}`); // Renombrar para evitar colisiones
                  await fs.copy(src, dest);
                  console.log(`Copiado asset adicional: ${src} -> ${dest}`);
              }
          }
      }
    }
    console.log('Copia de assets de Module Federation completada.');
  } catch (error) {
    console.error('Error durante la copia de assets de Module Federation:', error);
    process.exit(1);
  }
}

copyMfAssets();