// Styling sheet
import './SideMenu.css';

// Components
import Icon from '../Icon/Icon';
import Calendar from '../Calendar/Calendar';

function SideMenu({ openSection, setOpenSection, appointments, menuItems }) {
    const title = 'Danh mục chức năng';

    const toggle = (sectionId) => {
        setOpenSection((prev) => (prev === sectionId ? null : sectionId));
    };

    return (
        <div className='doctor-menu'>
            <div className='menu'>
                <div className='title'>{title}</div>

                {menuItems.map((item) => (
                    <button key={item.id} onClick={() => toggle(item.id)}>
                        <Icon src={item.icon} alt={`${item.id}-icon`} />
                        <div className='detail'>{item.label}</div>
                    </button>
                ))}
            </div>

            {appointments && (
                <div className='calendar'>
                    <Calendar appointments={appointments} />
                </div>
            )}
        </div>
    );
}

export default SideMenu;
