#include "DraggableFrame.h"
#include "ui_DraggableFrame.h"

DraggableFrame::DraggableFrame(QWidget *parent)
	: QFrame(parent, 0), m_ui(new Ui::DraggableFrame)
{
	m_ui->setupUi(this);
}

DraggableFrame::~DraggableFrame()
{

}