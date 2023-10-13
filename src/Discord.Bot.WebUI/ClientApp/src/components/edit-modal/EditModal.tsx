import { Modal } from "@mui/base";
import { Dialog, Popover, TextField } from "@mui/material";
import { useEffect, useState } from "react";
import Saying from "../../data/models/Saying";
import styles from './EditModal.module.scss';

const EditModal = ({ isModalOpen, saying, handleClose }: { isModalOpen: boolean, saying: Saying, handleClose: () => void }) => {
    const [open, setOpen] = useState(false);
    const [_internalSaying, setInternalSaying] = useState<Saying>(saying);

    useEffect(() => {
        setInternalSaying(saying);
        setOpen(isModalOpen);
    }, [saying])

    return (
        <>
            <Dialog
                open={open}
                onClose={handleClose}
                aria-labelledby="modal-modal-title"
                aria-describedby="modal-modal-description">

                <section className={ styles.container }>
                    <p>
                        {_internalSaying?.ismKey}
                    </p>
                    <TextField label="Ism Key" value={_internalSaying?.ismKey} />
                </section>

            </Dialog>

        </>
    )

}

export default EditModal;