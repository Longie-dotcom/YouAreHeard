import { useState } from 'react';
import './TabBar.css';

function TabBar({ tabs }) {
    const [activeTab, setActiveTab] = useState(0);

    const handleTabClick = (index, action) => {
        setActiveTab(index);
        if (typeof action === 'function') {
            action();
        }
    };

    return (
        <div className='tab-bar'>
            {tabs.map((tab, index) => (
                <button
                    key={index}
                    className={`tab-button ${activeTab === index ? 'active' : ''}`}
                    onClick={() => handleTabClick(index, tab.action)}
                >
                    {tab.name}
                </button>
            ))}
        </div>
    );
}

export default TabBar;
