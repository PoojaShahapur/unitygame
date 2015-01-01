#include "DraggableItemWidget.h"
#include "ui_DraggableItemWidget.h"
#include "QtAIEditorSys.h"
//#include <QtCore/QObject>

DraggableItemWidget::DraggableItemWidget(QWidget *parent)
	: QWidget(parent, 0), m_ui(new Ui::DraggableItemWidget)
{
	m_ui->setupUi(this);

	connect(m_ui->pushButton, SIGNAL(clicked()), this, SLOT(onClkBtn()));
}

DraggableItemWidget::~DraggableItemWidget()
{

}

void DraggableItemWidget::onClkBtn()
{
	AIEditorSys::getSingletonPtr();
}