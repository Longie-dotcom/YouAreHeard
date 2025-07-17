import React, { useState, useRef, useLayoutEffect, useEffect } from 'react';
import { ArrowRight, Heart, Users, Shield, Clock, Brain } from 'lucide-react';
import './HomePage.css';
import AnimatedLine from '../../component/AnimatedLine/AnimatedLine';

export default function HomePage() {
  const [hover, setHover] = useState(false);
  const [leftVideoHover, setLeftVideoHover] = useState(false);
  const [rightVideoHover, setRightVideoHover] = useState(false);
  const btnRef = useRef(null);
  const vidRef = useRef(null);
  const heroRef = useRef(null);
  const lineRef = useRef(null);
  const [lineStyle, setLineStyle] = useState({});
  const [lineAnimated, setLineAnimated] = useState(false);

  // Slide-up animation refs and state
  const titleRef = useRef(null);
  const subtitleRef = useRef(null);
  const buttonContainerRef = useRef(null);
  const [heroTextVisible, setHeroTextVisible] = useState(false);

  // Horizontal scroll cards refs
  const cardsContainerRef = useRef(null);
  const cardsScrollRef = useRef(null);

  // CTA animation refs and state
  const ctaTitleRef = useRef(null);
  const ctaSubtitleRef = useRef(null);
  const ctaButtonRef = useRef(null);
  const [ctaVisible, setCtaVisible] = useState(false);

  // Calculate video widths based on hover state and screen size
  const getVideoWidths = () => {
    const screenWidth = window.innerWidth;
    
    // Responsive base widths
    let baseWidth, expandedWidth, contractedWidth;
    
    if (screenWidth >= 1920) {
      // 27" and larger screens
      baseWidth = 800;
      expandedWidth = 900;
      contractedWidth = 700;
    } else if (screenWidth >= 1440) {
      // 16" MacBook Pro and similar
      baseWidth = 600;
      expandedWidth = 700;
      contractedWidth = 500;
    } else if (screenWidth >= 1200) {
      // 13" MacBook Pro and similar
      baseWidth = 500;
      expandedWidth = 600;
      contractedWidth = 400;
    } else {
      // Smaller screens
      baseWidth = 400;
      expandedWidth = 450;
      contractedWidth = 350;
    }
    
    if (leftVideoHover) {
      return { leftWidth: expandedWidth, rightWidth: contractedWidth };
    } else if (rightVideoHover) {
      return { leftWidth: contractedWidth, rightWidth: expandedWidth };
    } else {
      return { leftWidth: baseWidth, rightWidth: baseWidth };
    }
  };

  const { leftWidth, rightWidth } = getVideoWidths();

  // Helper to update line position
  const updateLine = () => {
    if (btnRef.current && vidRef.current && heroRef.current) {
      const btnRect = btnRef.current.getBoundingClientRect();
      const vidRect = vidRef.current.getBoundingClientRect();
      const lineContainerRect = lineRef.current.getBoundingClientRect();

      // Calculate left relative to the line container
      const left = btnRect.left - lineContainerRect.left;
      const right = vidRect.right - lineContainerRect.left;

      setLineStyle({
        position: 'absolute',
        left: left,
        width: right - left,
        top: 0,
        height: 0,
        borderTop: '1px solid #D0CFD3',
        zIndex: 2,
        pointerEvents: 'none',
        transition: 'width 0.8s cubic-bezier(.77,0,.18,1)'
      });
    }
  };

  useLayoutEffect(() => {
    if (
      btnRef?.current &&
      vidRef?.current &&
      heroRef?.current &&
      lineRef.current
    ) {
      updateLine();
      window.addEventListener('resize', updateLine);
      return () => window.removeEventListener('resize', updateLine);
    }
    // eslint-disable-next-line
  }, [btnRef, vidRef, heroRef, lineRef]);

  // Intersection Observer for scroll animation
  useEffect(() => {
    if (!lineRef.current) return;
    const observer = new window.IntersectionObserver(
      ([entry]) => {
        if (entry.isIntersecting) {
          setLineAnimated(true);
        }
      },
      { threshold: 0.3 }
    );
    observer.observe(lineRef.current);
    return () => observer.disconnect();
  }, []);

  // Slide-up animation trigger
  useEffect(() => {
    // Trigger animation immediately on component mount for better visibility
    const timer = setTimeout(() => {
      setHeroTextVisible(true);
    }, 200); // Small delay to ensure smooth rendering

    return () => clearTimeout(timer);
  }, []);

  // Optional: Keep the intersection observer as backup for when user scrolls back up
  useEffect(() => {
    if (!titleRef.current) return;
    const observer = new window.IntersectionObserver(
      ([entry]) => {
        if (entry.isIntersecting && !heroTextVisible) {
          setHeroTextVisible(true);
        }
      },
      { threshold: 0.1 } // Reduced threshold for earlier trigger
    );
    observer.observe(titleRef.current);
    return () => observer.disconnect();
  }, [heroTextVisible]);

  // Horizontal scroll effect for cards
  useEffect(() => {
    const handleScroll = () => {
      // Disable horizontal scroll on mobile
      if (window.innerWidth <= 480) {
        return;
      }
      
      if (!cardsContainerRef.current || !cardsScrollRef.current) return;

      const container = cardsContainerRef.current;
      const scrollContainer = cardsScrollRef.current;
      const containerRect = container.getBoundingClientRect();
      const windowHeight = window.innerHeight;
      
      // Calculate container center position
      const containerTop = containerRect.top;
      const containerHeight = containerRect.height;
      const containerCenter = containerTop + (containerHeight / 2);
      
      // Define middle zone of the screen (center 60% of viewport)
      const screenCenter = windowHeight / 2;
      const middleZoneHeight = windowHeight * 0.6;
      const middleZoneTop = screenCenter - (middleZoneHeight / 2);
      const middleZoneBottom = screenCenter + (middleZoneHeight / 2);
      
      // Check if container center is in the middle zone
      const isInMiddleZone = containerCenter >= middleZoneTop && containerCenter <= middleZoneBottom;
      
      if (isInMiddleZone) {
        // Calculate scroll progress within the middle zone
        const progressInMiddleZone = (containerCenter - middleZoneTop) / middleZoneHeight;
        const scrollProgress = Math.max(0, Math.min(1, progressInMiddleZone));
        
        // Calculate maximum scroll distance
        const containerWidth = container.offsetWidth;
        const scrollWidth = scrollContainer.scrollWidth;
        const maxScroll = Math.max(0, scrollWidth - containerWidth);
        
        // Apply horizontal scroll (LEFT TO RIGHT) - REVERSED
        const scrollLeft = (1 - scrollProgress) * maxScroll;
        scrollContainer.style.transform = `translateY(-50%) translateX(-${scrollLeft}px)`;
      } else {
        if (containerCenter < middleZoneTop) {
          // Before middle zone - show last card (rightmost position)
          const containerWidth = container.offsetWidth;
          const scrollWidth = scrollContainer.scrollWidth;
          const maxScroll = Math.max(0, scrollWidth - containerWidth);
          scrollContainer.style.transform = `translateY(-50%) translateX(-${maxScroll}px)`;
        } else if (containerCenter > middleZoneBottom) {
          // After middle zone - show first card (leftmost position)
          scrollContainer.style.transform = `translateY(-50%) translateX(0px)`;
        }
      }
    };

    window.addEventListener('scroll', handleScroll);
    handleScroll(); // Initial call
    
    return () => {
      window.removeEventListener('scroll', handleScroll);
    };
  }, []);

  // Add window resize listener to update video widths
  useEffect(() => {
    const handleResize = () => {
      // Force re-render on resize
      setLeftVideoHover(false);
      setRightVideoHover(false);
    };
    
    window.addEventListener('resize', handleResize);
    return () => window.removeEventListener('resize', handleResize);
  }, []);

  // CTA animation effect
  useEffect(() => {
    if (!ctaTitleRef.current) return;
    
    const observer = new window.IntersectionObserver(
      ([entry]) => {
        if (entry.isIntersecting) {
          setCtaVisible(true);
        }
      },
      { threshold: 0.3 } // Trigger when 30% of the element is visible
    );
    
    observer.observe(ctaTitleRef.current);
    return () => observer.disconnect();
  }, []);

  // Add this useEffect to ensure videos are visible on mobile
  useEffect(() => {
    const handleMobileVideoVisibility = () => {
      if (window.innerWidth <= 480) {
        // Force videos to be visible on mobile
        const heroVideo = vidRef.current;
        const videoContainers = document.querySelectorAll('.video-container');
        
        if (heroVideo) {
          heroVideo.style.display = 'block';
          heroVideo.style.visibility = 'visible';
          heroVideo.style.opacity = '1';
        }
        
        videoContainers.forEach(container => {
          container.style.display = 'block';
          container.style.visibility = 'visible';
          container.style.opacity = '1';
        });
        
        // Reset cards to vertical layout
        const cardsContainer = cardsScrollRef.current;
        if (cardsContainer) {
          cardsContainer.style.transform = 'none';
          cardsContainer.style.position = 'static';
        }
      }
    };

    handleMobileVideoVisibility();
    window.addEventListener('resize', handleMobileVideoVisibility);
    
    return () => window.removeEventListener('resize', handleMobileVideoVisibility);
  }, []);

  return (
    <div id="home-page" className="homepage-root">
      <div
        ref={heroRef}
        className="homepage-hero"
        style={{
          marginTop: '150px',
          position: 'relative',
          display: 'flex',
          alignItems: 'flex-start',
          minHeight: '700px'
        }}
      >
        <div className="homepage-hero-text" style={{ marginTop: '124px', flex: '0 0 auto' }}>
          <div
            ref={titleRef}
            className={`homepage-title homepage-slide-up${heroTextVisible ? ' visible' : ''}`}
          >
            Nếu bạn cần<br />giúp đỡ
          </div>
          <div
            ref={subtitleRef}
            className={`homepage-subtitle homepage-slide-up delay-100${heroTextVisible ? ' visible' : ''}`}
          >
            Hãy kể cho chúng tôi, chúng tôi sẵn sàng lắng nghe bạn.
          </div>
          <div
            ref={buttonContainerRef}
            className={`homepage-slide-up delay-200${heroTextVisible ? ' visible' : ''}`}
            style={{ marginTop: '40px' }}
          >
            <button
              ref={btnRef}
              className={`appointment-btn${hover ? ' appointment-btn-hover' : ''}`}
              onMouseEnter={() => setHover(true)}
              onMouseLeave={() => setHover(false)}
              onClick={() => window.location.href = '/register'}
              style={{ position: 'relative', border: 'none', background: 'none', padding: 0, outline: 'none', cursor: 'pointer' }}
            >
              <div className="appointment-btn-bg" />
              <span className="appointment-btn-text">Đặt lịch hẹn</span>
              <span className="appointment-btn-arrow">
                <ArrowRight size={28} color="#fff" />
              </span>
            </button>
          </div>
        </div>
        <div className="homepage-hero-image" style={{ flex: '0 0 auto', marginLeft: 'auto' }}>
          <video
            ref={vidRef}
            src={require('../../uploads/videos/4038501-uhd_2160_4096_25fps.mp4')}
            autoPlay
            loop
            muted
            playsInline
            style={{
              width: '720px',
              height: '646px',
              objectFit: 'cover',
              borderRadius: '24px',
              boxShadow: '0 4px 24px rgba(0,0,0,0.12)'
            }}
          />
        </div>
      </div>
      
      <AnimatedLine
        startRef={btnRef}
        endRef={vidRef}
        containerRef={heroRef}
        marginTop={32}
        className="homepage-animated-line-container"
      />

      <div className="homepage-section">
        <div className="homepage-section-label">
          SỨC KHỎE CỦA BẠN, CHÚNG TÔI LO
        </div>
        <div className="homepage-section-desc">
          Chúng tôi dành trọn tâm huyết để lắng nghe, thấu hiểu và đồng hành cùng bạn, mang đến sự chăm sóc chu đáo nhất trên hành trình sống khỏe mạnh và tự tin.
        </div>
      </div>
      
      <div className="video-section">
        <div className="video-section-container">
          <div className="video-wrapper">
            <div
              className="video-container"
              style={{
                width: `${leftWidth}px`
              }}
              onMouseEnter={() => setLeftVideoHover(true)}
              onMouseLeave={() => setLeftVideoHover(false)}
            >
              <video
                src={require('../../uploads/videos/66e7d4aca0aec2d3b22ba457_66eaca075f207c583e98e361_5487373-hd_1920_1080_25fps-transcode.mp4')}
                autoPlay
                loop
                muted
                playsInline
              />
              <div className="video-label">
                SỨC KHỎE TOÀN DIỆN
              </div>
              <div className="video-overlay-text">
                <h3 className="video-title">
                  Gia đình
                </h3>
              </div>
            </div>
          </div>

          <div className="video-wrapper right">
            <div
              className="video-container"
              style={{
                width: `${rightWidth}px`
              }}
              onMouseEnter={() => setRightVideoHover(true)}
              onMouseLeave={() => setRightVideoHover(false)}
            >
              <video
                src={require('../../uploads/videos/4625295-hd_1920_1080_25fps.mp4')}
                autoPlay
                loop
                muted
                playsInline
              />
              <div className="video-label">
                <strong>KHỎE ĐỂ KỀ VAI</strong>
              </div>
              <div className="video-overlay-text">
                <h3 className="video-title">
                  Bạn bè
                </h3>
              </div>
            </div>
          </div>
        </div>
      </div>

      {/* Animated line below the videos */}
      <AnimatedLine
        startRef={btnRef}
        endRef={vidRef}
        containerRef={heroRef}
        marginTop={80}
        className="homepage-animated-line-container"
      />

      {/* New section for the bottom text */}
      <div
        className="homepage-bottom-section"
        style={{
          marginTop: '40px',
          position: 'relative',
          display: 'flex',
          alignItems: 'flex-start',
          minHeight: '200px',
          maxWidth: '1920px',
          margin: '40px auto 0 auto',
          padding: '0 20px'
        }}
      >
        <div className="homepage-bottom-text" style={{ marginTop: '0px', flex: '0 0 auto' }}>
          <div
            className="homepage-title"
            style={{
              color: '#000',
              fontFamily: '"LT Superior Serif", serif',
              fontSize: '56px',
              fontStyle: 'medium',
              fontWeight: 500,
              lineHeight: '70px',
              textAlign: 'left',
              marginLeft: window.innerWidth <= 480 ? '0px' : '100px', // Add conditional margin
              marginTop: '30px',
            }}
          >
            Chúng tôi ở đây <br/> Ngay bên cạnh bạn.
          </div>
        </div>
      </div>

      {/* Horizontal Scroll Cards Section */}
      <div className="homepage-cards-section">
        <div ref={cardsContainerRef} className="homepage-cards-container">
          <div ref={cardsScrollRef} className="homepage-cards">
            <div className="homepage-card">
              <div className="homepage-card-icon">
                <Clock />
              </div>
              <div className="homepage-card-title">
                Hỗ trợ y tế 24/7
              </div>
              <div className="homepage-card-desc">
                Đội ngũ y bác sĩ chuyên khoa luôn sẵn sàng tư vấn và hỗ trợ bạn mọi lúc, mọi nơi, đảm bảo sự chăm sóc liên tục và kịp thời.
              </div>
            </div>

            <div className="homepage-card">
              <div className="homepage-card-icon">
                <Heart />
              </div>
              <div className="homepage-card-title">
                Kế hoạch chăm sóc cá nhân
              </div>
              <div className="homepage-card-desc">
                Phác đồ điều trị được thiết kế riêng cho từng cá nhân, phù hợp với tình trạng sức khỏe và lối sống của bạn.
              </div>
            </div>

            <div className="homepage-card">
              <div className="homepage-card-icon">
                <Users />
              </div>
              <div className="homepage-card-title">
                Hỗ trợ sức khỏe toàn diện
              </div>
              <div className="homepage-card-desc">
                Chăm sóc không chỉ về thể chất mà còn về tinh thần, dinh dưỡng và lối sống, giúp bạn có cuộc sống cân bằng và hạnh phúc.
              </div>
            </div>

            <div className="homepage-card">
              <div className="homepage-card-icon">
                <Brain />
              </div>
              <div className="homepage-card-title">
                Tư vấn tâm lý bằng tiếng Việt
              </div>
              <div className="homepage-card-desc">
                Được lắng nghe và chia sẻ bằng tiếng mẹ đẻ với các chuyên gia tâm lý am hiểu văn hóa Việt Nam.
              </div>
            </div>

            <div className="homepage-card">
              <div className="homepage-card-icon">
                <Shield />
              </div>
              <div className="homepage-card-title">
                Cộng đồng hỗ trợ an toàn
              </div>
              <div className="homepage-card-desc">
                Kết nối với cộng đồng những người có cùng hoàn cảnh trong môi trường an toàn, bảo mật và đầy tình thương.
              </div>
            </div>
          </div>
        </div>
      </div>

      {/* Call to Action Section */}
      <div className="homepage-cta-section">
        <div className="homepage-cta-container">
          <div className="homepage-cta-content">
            <h2 
              ref={ctaTitleRef}
              className={`homepage-cta-title homepage-cta-slide-up${ctaVisible ? ' visible' : ''}`}
            >
              Bắt đầu hành trình với YouAreHeard
            </h2>
            <p 
              ref={ctaSubtitleRef}
              className={`homepage-cta-subtitle homepage-cta-slide-up delay-100${ctaVisible ? ' visible' : ''}`}
            >
              Hành trình chăm sóc sức khỏe của bạn bắt đầu từ đây. Hãy cùng YouAreHeard trải nghiệm dịch vụ y tế hiện đại, được <br/>thiết kế để lắng nghe và hỗ trợ mọi người.
            </p>
            <button 
              ref={ctaButtonRef}
              className={`homepage-cta-button homepage-cta-slide-up delay-200${ctaVisible ? ' visible' : ''}`}
              onClick={() => window.location.href = '/register'}
            >
              <span className="homepage-cta-button-text">Bắt đầu ngay</span>
            </button>
          </div>
        </div>
      </div>

      {/* Footer */}
      <footer className="homepage-footer">
        <div className="homepage-footer-container">
          <div className="homepage-footer-content">
            {/* Logo and Trust Message */}
            <div className="homepage-footer-left">
              <div className="homepage-footer-logo">
                <div className="homepage-footer-logo-icon">✦</div>
                <span className="homepage-footer-logo-text">YouAreHeard</span>
              </div>
              
              <div className="homepage-footer-trust-message">
                <p className="homepage-footer-trust-text">
                  Chúng tôi tin rằng mọi người đều xứng đáng được lắng nghe và chăm sóc. 
                  Đội ngũ chuyên gia tâm lý của chúng tôi luôn sẵn sàng đồng hành cùng bạn 
                  trong mọi hoàn cảnh, với sự tôn trọng, bảo mật và tình thương.
                </p>
                <p className="homepage-footer-commitment">
                  <strong>Cam kết của chúng tôi:</strong> Bảo mật tuyệt đối, chăm sóc chuyên nghiệp, 
                  và luôn đặt sức khỏe tinh thần của bạn lên hàng đầu.
                </p>
              </div>
            </div>

            {/* Links */}
            <div className="homepage-footer-links">
              <div className="homepage-footer-column">
                <h4 className="homepage-footer-column-title">DỊCH VỤ</h4>
                <ul className="homepage-footer-column-list">
                  <li><a href="/login">Tư vấn tâm lý</a></li>
                  <li><a href="/login">Hỗ trợ khủng hoảng</a></li>
                  <li><a href="/login">Trị liệu nhóm</a></li>
                  <li><a href="/login">Tư vấn gia đình</a></li>
                </ul>
              </div>

              <div className="homepage-footer-column">
                <h4 className="homepage-footer-column-title">THÔNG TIN</h4>
                <ul className="homepage-footer-column-list">
                  <li><a href="/blogPage">Blog</a></li>
                </ul>
              </div>

              <div className="homepage-footer-column">
                <h4 className="homepage-footer-column-title">TÀI KHOẢN</h4>
                <ul className="homepage-footer-column-list">
                  <li><a href="/login">Đăng nhập</a></li>
                  <li><a href="/register">Đăng ký</a></li>
                </ul>
              </div>
            </div>
          </div>
          
          {/* Copyright */}
          <div className="homepage-footer-bottom">
            <div className="homepage-footer-copyright">
              <p>&copy; 2025 YouAreHeard. Tất cả quyền được bảo lưu.</p>
              <p className="homepage-footer-designer">
                Designed by <a href="https://github.com/BeforeLights" target="_blank" rel="noopener noreferrer">Acharné</a>
              </p>
            </div>
          </div>
        </div>
      </footer>
    </div>
  );
}