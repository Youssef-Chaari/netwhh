const express = require('express');
const path = require('path');
const cors = require('cors');

const app = express();
app.use(cors());

// Serve static files from the Angular build directory
// In Angular 19, the build output is typically in dist/[project-name]/browser
const buildPath = path.join(__dirname, 'dist', 'bi-project.ui', 'browser');
app.use(express.static(buildPath));

app.get('*', (req, res) => {
    res.sendFile(path.join(buildPath, 'index.html'));
});

const PORT = 4200;
app.listen(PORT, '127.0.0.1', () => {
    console.log(`Server running at http://127.0.0.1:${PORT}/`);
    console.log(`Serving from: ${buildPath}`);
});
