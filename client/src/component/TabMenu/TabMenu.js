// Styling sheet
import './TabMenu.css';

// Optional icon support
import Icon from '../Icon/Icon';

function TabMenu({ title, tabs, activeTabId, setActiveTabId }) {
    const handleClick = (id, action) => {
        setActiveTabId(id);
        if (typeof action === 'function') {
            action();
        }
    };

    return (
        <div className='tab-bar-container'>
            {title && <div className='tab-bar-title'>{title}</div>}
            <div className='tab-bar'>
                {tabs.map((tab) => (
                    <button
                        key={tab.id}
                        className={`tab-button ${activeTabId === tab.id ? 'active' : ''}`}
                        onClick={() => handleClick(tab.id, tab.action)}
                    >
                        {tab.icon && <Icon src={tab.icon} alt={`${tab.id}-icon`} />}
                        {tab.label}
                    </button>
                ))}
            </div>
        </div>
    );
}

export default TabMenu;