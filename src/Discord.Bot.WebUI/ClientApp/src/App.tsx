import { useEffect } from 'react';
import { useSelector } from 'react-redux';
import styles from './App.module.scss';
import { IsmTable } from './components/IsmTable';
import { PillSection } from './components/PillSection';
import { getAllSayingsAsyncThunk } from './data/store/IsmSlice';
import { AppDispatch, RootState } from './data/store/Store';

/**
 * Homepage
 * @returns
 */
export default function App() {
    const activeGuild = useSelector((state: RootState) => state.Guilds.activeGuild);
    const sayingsState = useSelector((state: RootState) => state.Isms)

    useEffect(() => {
        if (activeGuild) {
            // Grab all the isms to display them in a table
            AppDispatch(getAllSayingsAsyncThunk(activeGuild.id))
        }
    }, [activeGuild])

    return (
        <div className={styles.container}>
            <h1 className={styles.intro}>Isms Bot</h1>
            <section>
                <PillSection />

                <IsmTable sayings={sayingsState.value} />
                
            </section>
        </div >
    )
}

