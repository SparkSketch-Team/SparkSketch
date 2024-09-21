import React, { useEffect, useRef, useState } from 'react';
import './DrawingCanvas.css';

const DrawingCanvas = ({ onClose }) => {
  const canvasRef = useRef(null);
  const containerRef = useRef(null);
  const ctxRef = useRef(null);

  const [isDrawing, setIsDrawing] = useState(false);
  const [currentColor, setCurrentColor] = useState('#000000');
  const [currentOpacity, setCurrentOpacity] = useState(1);
  const [currentTool, setCurrentTool] = useState('pencil');
  const [currentSize, setCurrentSize] = useState(5);
  const [timeLeft, setTimeLeft] = useState(300);
  const [undoStack, setUndoStack] = useState([]);

  useEffect(() => {
    const canvas = canvasRef.current;
    const ctx = canvas.getContext('2d');
    ctxRef.current = ctx;

    // Set initial canvas size
    canvas.width = canvas.offsetWidth;
    canvas.height = canvas.offsetHeight;

    // Set initial background to white
    ctx.fillStyle = 'white';
    ctx.fillRect(0, 0, canvas.width, canvas.height);
  }, []);

  const handleMouseDown = (e) => {
    startDrawing(e);
  };

  const handleMouseMove = (e) => {
    draw(e);
  };

  const handleMouseUp = () => {
    stopDrawing();
  };

  const handleMouseOut = () => {
    stopDrawing();
  };

  useEffect(() => {
    const resizeCanvas = () => {
      if (containerRef.current && canvasRef.current) {
        const { width, height } = containerRef.current.getBoundingClientRect();
        canvasRef.current.width = width;
        canvasRef.current.height = height;
      }
    };

    resizeCanvas();
    window.addEventListener('resize', resizeCanvas);

    return () => window.removeEventListener('resize', resizeCanvas);
  }, []);

  useEffect(() => {
    const timer = setInterval(() => {
      setTimeLeft((prevTime) => {
        if (prevTime > 0) {
          return prevTime - 1;
        } else {
          clearInterval(timer);
          alert("It's been 5 minutes! Let's see what you've drawn!");
          return 0;
        }
      });
    }, 1000);

    return () => clearInterval(timer);
  }, []);

  const startDrawing = (e) => {
    setIsDrawing(true);
    saveCanvasState();
    draw(e);
  };

  const stopDrawing = () => {
    setIsDrawing(false);
    ctxRef.current.beginPath();
  };

  const draw = (e) => {
    if (!isDrawing) return;

    const canvas = canvasRef.current;
    const ctx = canvas.getContext('2d');
    const rect = canvas.getBoundingClientRect();
    const x = e.clientX - rect.left;
    const y = e.clientY - rect.top;

    ctx.lineWidth = currentSize;
    ctx.lineCap = 'round';

    if (currentTool === 'eraser') {
      ctx.globalCompositeOperation = 'destination-out';
      ctx.strokeStyle = 'rgba(0,0,0,1)';
    } else {
      ctx.globalCompositeOperation = 'source-over';
      ctx.strokeStyle = `${currentColor}${Math.round(currentOpacity * 255).toString(16).padStart(2, '0')}`;
    }

    ctx.lineTo(x, y);
    ctx.stroke();
    ctx.beginPath();
    ctx.moveTo(x, y);
  };

  //!Alternate version of drawing
  // const draw = (e) => {
  //   if (!isDrawing) return;

  //   const canvas = canvasRef.current;
  //   const ctx = canvas.getContext('2d');
  //   const rect = canvas.getBoundingClientRect();
  //   const x = e.clientX - rect.left;
  //   const y = e.clientY - rect.top;

  //   const imageData = ctx.getImageData(0, 0, canvas.width, canvas.height);
  //   const radius = currentSize / 2;

  //   for (let dx = -radius; dx < radius; dx++) {
  //     for (let dy = -radius; dy < radius; dy++) {
  //       const distance = Math.sqrt(dx*dx + dy*dy);
  //       if (distance < radius) {
  //         const pixelX = Math.round(x + dx);
  //         const pixelY = Math.round(y + dy);
  //         const index = (pixelY * canvas.width + pixelX) * 4;

  //         if (currentTool === 'eraser') {
  //           // Set pixel to fully transparent
  //           imageData.data[index + 3] = 0;
  //         } else {
  //           // Draw with current color and opacity
  //           const [r, g, b] = hexToRgb(currentColor);
  //           const a = Math.round(currentOpacity * 255);
  //           imageData.data[index] = r;
  //           imageData.data[index + 1] = g;
  //           imageData.data[index + 2] = b;
  //           imageData.data[index + 3] = a;
  //         }
  //       }
  //     }
  //   }

  //   ctx.putImageData(imageData, 0, 0);
  // };

  // // Helper function to convert hex to RGB
  // const hexToRgb = (hex) => {
  //   const r = parseInt(hex.slice(1, 3), 16);
  //   const g = parseInt(hex.slice(3, 5), 16);
  //   const b = parseInt(hex.slice(5, 7), 16);
  //   return [r, g, b];
  // };


  const saveCanvasState = () => {
    const canvas = canvasRef.current;
    setUndoStack(prevStack => [...prevStack, canvas.toDataURL()]);
  };

  const clearCanvas = () => {
    saveCanvasState();
    const canvas = canvasRef.current;
    const ctx = ctxRef.current;
    ctx.fillStyle = 'white';
    ctx.fillRect(0, 0, canvas.width, canvas.height);
  };

  const undo = () => {
    if (undoStack.length > 0) {
      const canvas = canvasRef.current;
      const ctx = canvas.getContext('2d');
      const img = new Image();
      img.onload = () => {
        ctx.clearRect(0, 0, canvas.width, canvas.height);
        ctx.drawImage(img, 0, 0);
      };
      img.src = undoStack[undoStack.length - 1];
      setUndoStack(prevStack => prevStack.slice(0, -1));
    }
  };

  const handleColorChange = (e) => {
    setCurrentColor(e.target.value);
  };

  const handleOpacityChange = (e) => {
    setCurrentOpacity(Number(e.target.value));
  };

  const floodFill = (x, y, fillColor) => {
    const canvas = canvasRef.current;
    const ctx = canvas.getContext('2d');
    const imageData = ctx.getImageData(0, 0, canvas.width, canvas.height);
    const width = imageData.width;
    const height = imageData.height;
    const stack = [[x, y]];
    const targetColor = getPixel(imageData, x, y);
    const fillColorRgba = hexToRgba(fillColor, currentOpacity);
    while (stack.length > 0) {
      const [x, y] = stack.pop();
      if (x < 0 || x >= width || y < 0 || y >= height) continue;
      if (!colorMatch(getPixel(imageData, x, y), targetColor)) continue;
      setPixel(imageData, x, y, fillColorRgba);
      stack.push([x + 1, y], [x - 1, y], [x, y + 1], [x, y - 1]);
    }
    ctx.putImageData(imageData, 0, 0);
  };
  const hexToRgba = (hex, opacity) => {
    const r = parseInt(hex.slice(1, 3), 16);
    const g = parseInt(hex.slice(3, 5), 16);
    const b = parseInt(hex.slice(5, 7), 16);
    const a = Math.round(opacity * 255);
    return [r, g, b, a];
  };

  const colorMatch = (color1, color2) => {
    return color1[0] === color2[0] &&
      color1[1] === color2[1] &&
      color1[2] === color2[2] &&
      Math.abs(color1[3] - color2[3]) < 5;
  };

  const getPixel = (imageData, x, y) => {
    const index = (y * imageData.width + x) * 4;
    return imageData.data.slice(index, index + 4);
  };

  const setPixel = (imageData, x, y, color) => {
    const index = (y * imageData.width + x) * 4;
    imageData.data.set(color, index);
  };

  const handleEyeDropper = (e) => {
    const canvas = canvasRef.current;
    const ctx = canvas.getContext('2d');
    const rect = canvas.getBoundingClientRect();
    const x = e.clientX - rect.left;
    const y = e.clientY - rect.top;
    const imageData = ctx.getImageData(x, y, 1, 1);
    const [r, g, b, a] = imageData.data;
    setCurrentColor(`#${r.toString(16).padStart(2, '0')}${g.toString(16).padStart(2, '0')}${b.toString(16).padStart(2, '0')}`);
    setCurrentOpacity(a / 255);
  };

  const handleCanvasClick = (e) => {
    if (currentTool === 'eyedropper') {
      handleEyeDropper(e);
    } else if (currentTool === 'paintbucket') {
      const canvas = canvasRef.current;
      const rect = canvas.getBoundingClientRect();
      const x = Math.floor(e.clientX - rect.left);
      const y = Math.floor(e.clientY - rect.top);
      floodFill(x, y, currentColor);
    }
  };

  const saveImage = () => {
    const canvas = canvasRef.current;
    const link = document.createElement('a');
    link.download = 'my-drawing.png';
    link.href = canvas.toDataURL('image/png');
    link.click();
  };

  return (
    <div className="game-container">
      <div className="timer">Time left: {timeLeft}s</div>
      <div className="header">HEY, IT'S TIME TO DRAW!</div>
      <div className="canvas-container" ref={containerRef}>
        <canvas
          ref={canvasRef}
          onMouseDown={handleMouseDown}
          onMouseMove={handleMouseMove}
          onMouseUp={handleMouseUp}
          onMouseOut={handleMouseOut}
          onClick={handleCanvasClick}
        />
      </div>
      <div className="tools">
        <div className="color-palette">
          {['#000000', '#ffffff', '#ff0000', '#00ff00', '#0000ff', '#ffff00', '#ff00ff', '#00ffff', '#ffa500', '#800080'].map((color) => (
            <div
              key={color}
              className="color-option"
              style={{ backgroundColor: color }}
              onClick={() => { setCurrentColor(color); setCurrentTool('pencil'); }}
            />
          ))}
        </div>
        <div className="drawing-tools">
          <button className={`tool-btn ${currentTool === 'pencil' ? 'active' : ''}`} onClick={() => setCurrentTool('pencil')}>✏️</button>
          <button className={`tool-btn ${currentTool === 'eraser' ? 'active' : ''}`} onClick={() => setCurrentTool('eraser')}>🧽</button>
          <button className={`tool-btn ${currentTool === 'paintbucket' ? 'active' : ''}`} onClick={() => setCurrentTool('paintbucket')} title="Paint Bucket">🪣</button>
          <button className={`tool-btn ${currentTool === 'eyedropper' ? 'active' : ''}`} onClick={() => setCurrentTool('eyedropper')} title="Eye Dropper">👁️</button>
          {/* <div className="color-picker-container"> */}
          <label htmlFor="colorPicker" className="tool-btn" title="Color Picker">🎨</label>
          <input type="color" value={currentColor} onChange={handleColorChange} id="colorPicker" />
          {/* </div> */}
          <span className="size-label">Opacity:</span>
          <input
            type="range"
            min="0"
            max="1"
            step="0.01"
            value={currentOpacity}
            className="size-slider"
            onChange={handleOpacityChange}
          />
          <span className="size-label">Size:</span>
          <input
            type="range"
            min="1"
            max="50"
            value={currentSize}
            className="size-slider"
            onChange={(e) => setCurrentSize(Number(e.target.value))}
          />
          <button className="tool-btn" onClick={undo} title="Undo">↩️</button>
          <button className="tool-btn" onClick={clearCanvas}>🗑️</button>
        </div>
      </div>
      <div className="button-container">
        <button className="App-upload" onClick={saveImage}>Save Image</button>
        <button className="App-upload" onClick={onClose}>Close</button>
      </div>
    </div>
  );
};

export default DrawingCanvas;