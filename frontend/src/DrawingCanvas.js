import React, { useEffect, useRef, useState } from 'react';
import './DrawingCanvas.css';

const DrawingCanvas = ({ onClose }) => {
  const canvasRef = useRef(null);
  const containerRef = useRef(null);
  const [isDrawing, setIsDrawing] = useState(false);
  const [currentColor, setCurrentColor] = useState('#000000');
  const [currentTool, setCurrentTool] = useState('pencil');
  const [currentSize, setCurrentSize] = useState(5);
  const [timeLeft, setTimeLeft] = useState(300);

  useEffect(() => {
    const resizeCanvas = () => {
      if (containerRef.current && canvasRef.current) {
        const { width, height } = containerRef.current.getBoundingClientRect();
        canvasRef.current.width = width;
        canvasRef.current.height = height;
        const ctx = canvasRef.current.getContext('2d');
        ctx.fillStyle = 'transparent';
        ctx.fillRect(0, 0, width, height);
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
    draw(e);
  };

  const stopDrawing = () => {
    setIsDrawing(false);
    const canvas = canvasRef.current;
    const ctx = canvas.getContext('2d');
    ctx.beginPath();
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
    } else {
      ctx.globalCompositeOperation = 'source-over';
      ctx.strokeStyle = currentColor;
    }

    ctx.lineTo(x, y);
    ctx.stroke();
    ctx.beginPath();
    ctx.moveTo(x, y);
  };

  const clearCanvas = () => {
    const canvas = canvasRef.current;
    const ctx = canvas.getContext('2d');
    ctx.clearRect(0, 0, canvas.width, canvas.height);
    ctx.fillStyle = 'white';
    ctx.fillRect(0, 0, canvas.width, canvas.height);
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
          onMouseDown={startDrawing}
          onMouseMove={draw}
          onMouseUp={stopDrawing}
          onMouseOut={stopDrawing}
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
          <button className="tool-btn" onClick={() => setCurrentTool('pencil')}>✏️</button>
          <button className="tool-btn" onClick={() => setCurrentTool('eraser')}>🧽</button>
          <span className="size-label">Size:</span>
          <input
            type="range"
            min="1"
            max="50"
            value={currentSize}
            className="size-slider"
            onChange={(e) => setCurrentSize(Number(e.target.value))}
          />
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