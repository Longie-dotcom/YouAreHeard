import React, { useRef, useState, useLayoutEffect, useEffect } from 'react';
import './AnimatedLine.css';

export default function AnimatedLine({
  startRef,
  endRef,
  containerRef,
  className = '',
  style = {},
  trigger = 'scroll',
  marginTop = 0,
  borderColor = '#D0CFD3',
  borderWidth = 1,
  transition = 'width 0.8s cubic-bezier(.77,0,.18,1)'
}) {
  const lineRef = useRef(null);
  const [lineStyle, setLineStyle] = useState({ left: 0, width: 0 });
  const [animated, setAnimated] = useState(false);
  const [targetWidth, setTargetWidth] = useState(0);

  // Calculate line position and width
  const updateLine = () => {
    if (
      startRef?.current &&
      endRef?.current &&
      containerRef?.current &&
      lineRef.current
    ) {
      const startRect = startRef.current.getBoundingClientRect();
      const endRect = endRef.current.getBoundingClientRect();
      const containerRect = lineRef.current.getBoundingClientRect();

      const left = startRect.left - containerRect.left;
      const right = endRect.right - containerRect.left;
      const width = right - left;

      setLineStyle({
        left,
        borderTop: `${borderWidth}px solid ${borderColor}`,
        transition,
      });
      setTargetWidth(width);
    }
  };

  useLayoutEffect(() => {
    updateLine();
    window.addEventListener('resize', updateLine);
    return () => window.removeEventListener('resize', updateLine);
    // eslint-disable-next-line
  }, [startRef, endRef, containerRef, borderColor, borderWidth, transition]);

  // Animation trigger
  useEffect(() => {
    if (trigger === 'scroll') {
      if (!lineRef.current) return;
      const observer = new window.IntersectionObserver(
        ([entry]) => {
          if (entry.isIntersecting) setAnimated(true);
        },
        { threshold: 0.3 }
      );
      observer.observe(lineRef.current);
      return () => observer.disconnect();
    } else {
      setAnimated(true);
    }
  }, [trigger]);

  return (
    <div
      ref={lineRef}
      className={`animated-line-container ${className}`}
      style={{ marginTop, ...style }}
    >
      <div
        className={`homepage-animated-line${animated ? ' animated' : ''}`}
        style={{
          ...lineStyle,
          width: animated ? targetWidth : 0,
        }}
      />
    </div>
  );
}